using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Linked components
    private CharacterController2D cc2d;             // The Movement controller attached on player
    private Animator animator;                      // The animator attached on the player
    public Player player;                      // The animator attached on the player

    private int direction = 0;                      // Value indicating whether the player is moving to the right, to the left or is just static
    [SerializeField] private float acceleration;    // Controller movement accelleration (how fast the character will reach the maximum speed

    //Keyboard Informations
    private float horizontalMove = 0.0f;            // Used to give him speed in that direction (Keyboard) 

    //Controller Informations
    private float lastValue;                        // To Check when the player flips the character (Controller)
    private float controllerHorizontalValue = 0.0f; // Used to give him speed in that direction (Controller) 

    //Slope Information
    ContactPoint2D[] contacts;
    float slopeAngle;
    float maxSlopeAngle = 60;
    RaycastHit2D hit;
   
    //Immobile on slopes
    float counter;
    float time = 0.1f;
    bool immobile = false;

    //New Movement System
    public float jumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float TimeToJumpApex = 0.4f;
    public float moveSpeed = 9;

    private float yDistance;
    private bool jumpReleased;


    float gravity;
    float jumpVelocity;
    Vector3 velocity;


    [Header("- Coyote Time Settings -")]
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeAfterLeavingGround = 0.1f;      //CoyoteTime after Leaving the ground
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeBeforeReachingGround = 0.1f;    //CoyoteTime before reaching the ground

    //Coyote Time Counters
    private float groundedTimeCount = 0.0f;         // Counts the last time the character was in contact with the floor
    private float jumpPressedTimeCount = 0.0f;      // Counts the last time the player pushed the jump button

    //Jump Information
    private bool onAir = false;                     // Determines if the player is currently in the air or not
    private bool jumpValidation = false;            // Determines if the player is allowed to jump in his current situation
    public bool Inactive = false;                  // Determines if the player is in his invincibility frames or not
    public bool Interacting = false;               // Determines if the player is interacting with some entity
    public bool canMove = true;

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
        gravity = -(2 * jumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;

        stopCharacterFromSliding();

        if(cc2d.collisions.above || cc2d.collisions.below)
        {
            velocity.y = 0;
            cc2d.OnLandEvent.Invoke();
        }

        float horizontal = calculateDirection();
        
        Vector2 input = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && cc2d.collisions.below
            || Input.GetButton("Jump") && cc2d.collisions.below && jumpValidation)
        {
            velocity.y = jumpVelocity;
            animator.SetBool("jump", true);
        }

        if (Input.GetButtonUp("Jump") && !cc2d.collisions.below && velocity.y >= 0)
        {
            if(yDistance >= minJumpHeight)
            {
                velocity.y = 0;
            }
            else
            {
                jumpReleased = true;
            }
        }

        if (velocity.y >= 0 && jumpReleased && yDistance >= minJumpHeight)
        {
            jumpReleased = false;
            velocity.y = 0;
        }


        if (!player.isDead && !player.GM.isPaused && !Interacting)
        {

            velocity.x = input.x * moveSpeed;

            velocity.y += gravity * Time.deltaTime;

            cc2d.newMove(velocity * Time.deltaTime);

            Actions();
            applyMovement();
            animator.SetFloat("speed", Mathf.Abs(horizontal));

            //Coyote Time prototype
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
    }

    void FixedUpdate()
    {
        //Move the character

        if (!player.isDead && !Inactive && !Interacting && canMove && !player.GM.isPaused)
        {

            /*int layer_mask = LayerMask.GetMask("Ground");
            hit = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, 100, layer_mask);

            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);*/
            cc2d.Move(horizontalMove, onAir);
            

            /*if (!isJumping() && !immobile)
            {
                
                if (hit.normal != Vector2.up && slopeAngle <= maxSlopeAngle && slopeAngle >= 3)
                {
                    gameObject.transform.position -= new Vector3(0, Mathf.Abs(hit.point.y - raycastOrigins.bottomRight.y), 0);
                }
                
            }   */
        }
    }

    //Movements to apply depending on which controller is used
    void applyMovement()
    {
        horizontalMoveResult();
        ControllerMovements();
    }

    //Keyboard Movements value calculation
    void horizontalMoveResult()
    {
        horizontalMove = direction;
    }

    //Controller Movements value calculation
    void ControllerMovements()
    {
        controllerHorizontalValue = Input.GetAxisRaw("Horizontal");

        if(controllerHorizontalValue >= 0.1f)
        {
            horizontalMove = 1 * acceleration;
        }
        else if(controllerHorizontalValue <= -0.1f)
        {
            horizontalMove = -1 * acceleration;
        }
        lastValue = horizontalMove;
    }


    //Character actions
    void Actions()
    {
        if ( Input.GetButtonDown("Jump") && cc2d.getGrounded() && canMove) 
        {
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        if ((Input.GetButtonDown("Jump") && !onAir && jumpValidation && canMove) 
            || (Input.GetButton("Jump") && cc2d.getGrounded() && jumpValidation && canMove) 
            || (Input.GetButton("Jump") && !onAir && jumpValidation && canMove))
        {
            //onAir = true;
            //animator.SetBool("jump", true);
        }

    }

    //Function called when the character hit the ground
    public void OnLanding()
    {
        yDistance = 0;
        animator.SetBool("jump", false);
        cc2d.ResetJump();
        onAir = false;
    }

    //jump getter
    public bool isJumping()
    {
        return onAir;
    }

    //direction getter
    public int getDirection()
    {
        return direction;
    }

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

    private void stopCharacterFromSliding()
    {
        //Stop character from sliding on slopes
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

    public void CalculateYDistance(float value)
    {
        yDistance += value;
    }

}