using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Linked components
    private CharacterController2D cc2d;             // The Movement controller attached on player
    public Animator animator;                      // The animator attached on the player
    public Player player;                           // The animator attached on the player

    //Movement Information
    float horizontalMove = 0.0f;
    float gravity;
    Vector3 velocity;

    //Jump information
    float yDistance;
    bool jumpReleased;
    public bool jumped = false;
    float maxJumpVelocity;
    public bool ableToMove = false;                  // Determines if the player inputs are recorded or not
    public bool Interacting = false;               // Determines if the player is interacting with some entity
    public bool canMove = true;

    //Immobile on slopes
    float counter;
    float time = 0.1f;
    bool immobile = false;   

    [Header("- Player Movement Settings -")]
    public float maxJumpHeight = 8f;
    public float minJumpHeight = 1f;
    public float TimeToJumpApex = 0.4f;
    public float moveSpeed = 9;

    bool gotHit = false;


    [Header("- Coyote Time Settings -")]
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeAfterLeavingGround = 0.1f;      //CoyoteTime after Leaving the ground
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeBeforeReachingGround = 0.1f;    //CoyoteTime before reaching the ground

    //Coyote Time Information
    private float groundedTimeCount = 0.0f;         // Counts the last time the character was in contact with the floor
    private float jumpPressedTimeCount = 0.0f;      // Counts the last time the player pushed the jump button
    private bool jumpValidation = false;            // Determines if the player is allowed to jump in his current situation


    // Awake is called once before Start
    void Awake()
    {
        cc2d = GetComponent<CharacterController2D>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        /*gravity = -(2 * jumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;*/
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!player.GM.isPaused)
        {
            gravity = -(2 * maxJumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;

            stopCharacterFromSliding();

            if((cc2d.collisions.above || cc2d.collisions.below) && !gotHit)
            {
                velocity.y = 0;
                cc2d.OnLandEvent.Invoke();
            }

            horizontalMove = calculateDirection();
            if (!gotHit && !player.GetDead() && !Interacting && !GetComponent<CombatManager>().isHealing)
            {
                Jump();
                CoyoteTime(); 
            }
            playerMovements();
        }
    }

    void FixedUpdate()
    {
        //Turn the character on the direction he is facing
        if (!player.GetDead() && !ableToMove && !Interacting && canMove && !player.GM.isPaused)
        {
            cc2d.Turn(horizontalMove);
        }
    }

    //Function called when the character hit the ground
    public void OnLanding()
    {
        yDistance = 0;
        jumped = false;
        velocity.x = 0;
        animator.SetBool("jump", false);
        animator.SetBool("fall", false);
        cc2d.ResetJump();
    }

    //Calculate direction faced depending on controller/keyboard information
    private float calculateDirection()
    {
        float horizontal;
        if (Input.GetAxisRaw("Horizontal") >= 0.1f)
        {
            horizontal = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") <= -0.1f)
        {
            horizontal = -1;

        }
        else
        {
            horizontal = 0;
        }

        return horizontal;
    }

    //Stop character from sliding on slopes
    private void stopCharacterFromSliding()
    {
        if (horizontalMove == 0)
        {
            counter += Time.deltaTime;
            if (counter >= time)
            {
                immobile = true;
            }
        }
        else
        {
            immobile = false;
            counter = 0;
        }
    }

    //Permits flexibility on jump input to smooth the player movements
    private void CoyoteTime()
    {
        groundedTimeCount -= Time.deltaTime;
        if (cc2d.getGrounded())
        {
            groundedTimeCount = coyoteTimeAfterLeavingGround;
        }

        jumpPressedTimeCount -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedTimeCount = coyoteTimeBeforeReachingGround;
        }

        if ((jumpPressedTimeCount > 0) && (groundedTimeCount > 0))
        {
            jumpValidation = true;
        }
        else
        {
            jumpValidation = false;
        }
    }

    //Handles jump inputs
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && cc2d.collisions.below
            || Input.GetButton("Jump") && cc2d.collisions.below && jumpValidation)
        {
            jumped = true;
            velocity.y = maxJumpVelocity;
            animator.SetBool("jump", true);
        }

        if (Input.GetButtonUp("Jump") && !cc2d.collisions.below && velocity.y >= 0 && jumped)
        {
            if (yDistance >= minJumpHeight)
            {
                velocity.y = 0;
            }
            else
            {
                jumpReleased = true;
            }
        }

        if (!cc2d.collisions.below && velocity.y <= 0)
        {
            if (yDistance <= -0.5f)
            {
                animator.SetBool("fall", true);
            }
        }

        if (velocity.y >= 0 && jumpReleased && yDistance >= minJumpHeight)
        {
            jumpReleased = false;
            velocity.y = 0;
        }
    }

    //Move the player depending on inputs and 
    void playerMovements()
    {
        Vector2 input = new Vector2(0, 0);

        if (!player.GetDead() && !Interacting && !GetComponent<CombatManager>().isHealing && canMove)
        {
            input = new Vector2(horizontalMove, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            input = new Vector2(0, 0);
        }
        
        if (!gotHit)
        {
            velocity.x = input.x * moveSpeed;
        }
        velocity.y += gravity * Time.deltaTime;
        cc2d.newMove(velocity * Time.deltaTime);
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));
    }

    //Calculate height travelled during jump
    public void CalculateYDistance(float value)
    {
        yDistance += value;
    }

    public void StartKnockBack(Vector2 direction)
    {
            gotHit = true;
            animator.SetTrigger("gotHit");
            animator.SetBool("beingHit",true);
            velocity.y = direction.y;
            velocity.x = direction.x;
    }

    public void StopKnockBack()
    {
        gotHit = false;
        animator.SetTrigger("stopHit");
        animator.SetBool("beingHit", false);
        /*velocity.y = 0;
        velocity.x = 0;*/
    }

    public void setVelocity(float yVelocity)
    {
        velocity.y = yVelocity;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        animator.SetBool("canMove", value);
    }
}