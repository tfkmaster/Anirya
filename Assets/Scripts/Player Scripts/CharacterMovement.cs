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

    //New colision system
    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    const float skinWidth = 0.015f;
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    //New Movement System
    float gravity = -20;
    Vector3 velocity;
    public LayerMask layer_mask2;


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
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public void HorizontalCollisions(ref Vector3 velocity)
    {
        float DirectionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (DirectionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.right * DirectionX, rayLength, layer_mask2);
            Debug.DrawRay(rayOrigin, Vector2.right * DirectionX * rayLength, Color.red);
            if (hit2)
            {
                velocity.x = (hit2.distance - skinWidth) * DirectionX;
                rayLength = hit2.distance;
            }
        }
    }

    public void VerticalCollisions(ref Vector3 velocity)
    {
        float DirectionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (DirectionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);            
            
            RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.up * DirectionY, rayLength, layer_mask2);
            Debug.DrawRay(rayOrigin, Vector2.up * DirectionY * rayLength, Color.red);
            if (hit2)
            {
                Debug.Log("a");
                velocity.y = (hit2.distance-skinWidth) * DirectionY;
                rayLength = hit2.distance;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Stop character from sliding on slopes
        if (horizontalMove == 0)
        {
            counter += Time.deltaTime;
            if(counter >= time)
            {
                immobile = true;
            }
        }
        else
        {
            immobile = false;
            counter = 0;
        }

        float horizontal;
        if(Input.GetAxisRaw("Horizontal") >= 0.3f)
        {
            horizontal = 1;
        }
        else if(Input.GetAxisRaw("Horizontal")<= -0.3f)
        {
            horizontal = -1;

        }
        else
        {
            horizontal = 0;
        }
        Vector2 input = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * 9;
        velocity.y += gravity * Time.deltaTime;
        UpdateRaycastOrigins();
        cc2d.newMove(velocity * Time.deltaTime);

        if (!player.isDead && !player.GM.isPaused && !Interacting)
        {
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

        if(controllerHorizontalValue >= 0.3f)
        {
            horizontalMove = 1 * acceleration;
        }
        else if(controllerHorizontalValue <= -0.3f)
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

        if ((Input.GetButtonDown("Jump") && !onAir && jumpValidation && canMove) 
            || (Input.GetButton("Jump") && cc2d.getGrounded() && jumpValidation && canMove) 
            || (Input.GetButton("Jump") && !onAir && jumpValidation && canMove))
        {
            onAir = true;
            animator.SetBool("jump", true);
        }

    }

    //Function called when the character hit the ground
    public void OnLanding()
    {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*if (collision.otherCollider.GetType() == typeof(CircleCollider2D) && !collision.collider.GetComponent<Player>())
        {
            contacts = collision.contacts;
        }*/
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(gameObject.transform.position + new Vector3(0,1,0), new Vector3(hit.point.x, hit.point.y, 0));
    }
}