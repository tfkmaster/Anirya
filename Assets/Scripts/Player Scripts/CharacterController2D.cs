using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{

    [Space]

    public bool m_Grounded;            // Whether or not the player is grounded.

    [Space]

    [Header("Collision")]
    [Space]

    public float collisionRadius = 0.12f;
    private Color debugCollisionColor = Color.red;
    public CollisionInfo collisions;

    [Header("- Jump Settings -")]
    [Space]

    [SerializeField] private float m_JumpForce = 200f;                          // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping;
    [SerializeField] private float jumpHigh = 4;                                // How high the character can jump

    [Header("- Move Settings -")]
    [Space]

    [SerializeField] private float accelerationX = 0.1f;                        // The value of the acceleration to reach maxSpeed

    [Header("- EnvironmentInfo Settings -")]
    [Space]

    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Transform m_GroundCheck;                          // A position marking where to check for ground
    [SerializeField] private Transform m_Center;                               // Center of the player

    //New colision system
    BoxCollider2D footCollider;
    RaycastOrigins raycastOrigins;
    const float skinWidth = 0.015f;
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    private Rigidbody2D m_Rigidbody2D;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    
    //Movement Information
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.

    //Jump Informations
    private bool topReached = false; // For determining if the jump climax has been reached
    private float yPos; //Y pos the player was on the base of his jump
    private bool yPosRemembered = false; //For determining if the Y value on the base of the jump has been already stored  

    CharacterMovement characterMovement;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        //m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void Start()
    {
        footCollider = GetComponent<BoxCollider2D>();
        characterMovement = GetComponent<CharacterMovement>();
        CalculateRaySpacing();
    }

    

    public void newMove(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
        characterMovement.CalculateYDistance(velocity.y);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = footCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = footCollider.bounds;
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

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * DirectionX, rayLength, m_WhatIsGround);
            Debug.DrawRay(rayOrigin, Vector2.right * DirectionX * rayLength, Color.red);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * DirectionX;
                rayLength = hit.distance;

                collisions.left = DirectionX == -1;
                collisions.right = DirectionX == 1;
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

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * DirectionY, rayLength, m_WhatIsGround);
            Debug.DrawRay(rayOrigin, Vector2.up * DirectionY * rayLength, Color.red);
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * DirectionY;
                rayLength = hit.distance;

                collisions.below = DirectionY == -1;
                collisions.above = DirectionY == 1;
            }
        }
    }

    private void Update()
    {
        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, collisionRadius, m_WhatIsGround);

        if (Physics2D.OverlapCircle(m_CeilingCheck.position, collisionRadius, m_WhatIsGround) && !m_Grounded && !GetComponent<Player>().isOnOneWayPlatform)
        {
            topReached = true;
            Debug.Log("Top reached");
        }
        //Push the caracter to the ground if the jump button released before reaching it's climax
        //Allows to do more precise jumps
        if (Input.GetButtonUp("Jump") && !topReached)
        {
            topReached = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Allows to jump again after reaching the ground
        if (collision.gameObject.layer == 9 && !GetComponent<Player>().isOnOneWayPlatform && !collision.otherCollider.gameObject.CompareTag("One Way Platform") && m_Grounded)
        {
            OnLandEvent.Invoke();
        }

        //Prevent the player from jumping when colliding with platform sides
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            // check if the platform collided on the sides
            if (hitPos.normal.x != 0) 
            {
                // boolean to prevent player from being able to jump
                m_Grounded = false; 
                //collision.gameObject.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0f;
            }
            // check if its collided on top 
            else if (hitPos.normal.y > 0)                   
            {
                m_Grounded = true;
            }
            else
            {
                m_Grounded = false;
            } 
        }
    }

    public void Move(float direction, bool onAir)
    {

        if (!GetComponent<CharacterMovement>().Inactive)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl && !GetComponent<CharacterMovement>().Inactive)
            {
                //m_Rigidbody2D.velocity = new Vector2(direction * accelerationX / 10, m_Rigidbody2D.velocity.y);
                // If the input is moving the player right and the player is facing left...
                if (direction > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (direction < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player is on the air
            if (!topReached && onAir && Input.GetButton("Jump"))
            {
                //Remember the Y Pos on the base of the jump
                if (!yPosRemembered)
                {
                    yPos = GetComponent<Transform>().position.y;
                    yPosRemembered = true;
                }

                // Add a vertical force to the player and maintains it while the jumpClimax Hasn't been reached.
                if (GetComponent<Transform>().position.y <= (yPos + jumpHigh))
                {
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Vector2.up.y * m_JumpForce);
                }
                else
                {
                    topReached = true;
                    yPosRemembered = false;
                    return;
                }
            }
        }
    }

    //Flips the player model
    private void Flip()
    {
        if (!GetComponent<CharacterMovement>().Inactive)
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

        //Getter that returns the character rigidbody2D
        public Rigidbody2D getRigidbody()
    {
        return m_Rigidbody2D;
    }

    //Getter returning which direction the player is facing at
    // 1 for Right and -1 for Left
    public int facingDirection()
    {
        if (m_FacingRight)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    //Put the character velocity at 0
    public void stopCharacter()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.angularVelocity = 0;
    }

    public bool isStoped()
    {
        return m_Rigidbody2D.velocity == Vector2.zero;
    }



    //Reset Jump values, function called on Landing
    public void ResetJump()
    {
        topReached = false;
        m_Grounded = true;
        yPosRemembered = false;
    }

    //m_grounded boolean getter
    public bool getGrounded()
    {
        return m_Grounded;
    }

    //m_rigidbody2D boolean getter
    public Rigidbody2D getRigibody2D()
    {
        return m_Rigidbody2D;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(m_GroundCheck.position, collisionRadius);
        Gizmos.DrawWireSphere(m_CeilingCheck.position, collisionRadius);
        /*Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);*/
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    
}
