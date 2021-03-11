using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;
using System.Collections;

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

    [SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping;


    [Header("- Environment Info Settings -")]
    [Space]

    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_DefaultLayer;                          // A mask determining what is the default layer
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Transform m_GroundCheck;                          // A position marking where to check for ground
    [SerializeField] private Transform m_Center;                               // Center of the player

    //Collision Information
    BoxCollider2D groundCollider;
    RaycastOrigins raycastOrigins;
    const float skinWidth = 0.015f;
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    Vector2 rayOrigin;

    Vector3 a;
    Vector3 b;
    Vector3 c;
    Vector3 d;
    float characterAngleA;
    float characterAngleB;
    float actualAngle;
    float oldAngle;
    bool wasIdling = false;

    float rootFloat;
    float container;
    float arms;
    
    public GameObject root;
    public Transform leftArm;
    public Transform rightArm;

    //Slope Information
    ContactPoint2D[] contacts;
    float slopeAngle;
    float SignedSlopeAngle;
    public float maxClimbAngle = 60;
    public float maxDescendAngle = 60;
    RaycastHit2D hit;

    private Rigidbody2D m_Rigidbody2D;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    
    //Movement Information
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.
  
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
        groundCollider = GetComponent<BoxCollider2D>();
        characterMovement = GetComponent<CharacterMovement>();
        CalculateRaySpacing();
    }

    private void LateUpdate()
    {
        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, collisionRadius, m_WhatIsGround);
        /*if (characterMovement.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            wasIdling = true;
            RaycastHit2D hitSlope = Physics2D.Raycast(rayOrigin, Vector2.down, 5, m_WhatIsGround | m_DefaultLayer);

            if (hitSlope)
            {
                float signedAngle = (-1) * Vector2.SignedAngle(hitSlope.normal, Vector2.up);

                actualAngle = signedAngle;
                characterAngleA = root.GetComponent<RotationConstraint>().rotationOffset.z;

                
                characterAngleB = transform.GetChild(0).transform.localRotation.eulerAngles.z;


                if ((((characterAngleA <= -((facingDirection() * actualAngle + 2) % 360)) || (characterAngleA >= -((facingDirection() * actualAngle - 2) % 360)))) )
                {

                    if (actualAngle < 0 && signedAngle != 0f)
                    {
                        if (true)
                        {
                            a = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleA + (Time.deltaTime * 100) * facingDirection());
                            b = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleB - (Time.deltaTime * 100));                          
                            Debug.Log("a");
                        }
                        else
                        {
                            a = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleA + (Time.deltaTime * 100) * facingDirection());
                            b = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleB - (Time.deltaTime * 100));
                            Debug.Log("b");
                        }
                    }
                    else if (oldAngle < 180 && actualAngle < 180 && signedAngle != 0f)
                    {
                        if ((oldAngle <= actualAngle) && (-oldAngle <= actualAngle))
                        {
                            a = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleA - (Time.deltaTime * 100) * facingDirection());
                            b = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, characterAngleB + (Time.deltaTime * 100));
                            Debug.Log("c");
                        }
                        else
                        {
                            a = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y,  characterAngleA + (Time.deltaTime * 100)  *facingDirection());
                            b = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y,  characterAngleB - (Time.deltaTime * 100));
                            Debug.Log("d");
                        }
                    }

                    if(facingDirection() == -1)
                    {
                        c = new Vector3(leftArm.eulerAngles.x, leftArm.eulerAngles.y, characterAngleB + (Mathf.Sign(signedAngle) * facingDirection() * (Time.deltaTime * 100)));
                        d = new Vector3(rightArm.eulerAngles.x, rightArm.eulerAngles.y, characterAngleB + (Mathf.Sign(signedAngle) * facingDirection() * (Time.deltaTime * 100)));
                    }
                    else
                    {
                        c = new Vector3(leftArm.eulerAngles.x, leftArm.eulerAngles.y, - characterAngleB + ((Time.deltaTime * 100)));
                        d = new Vector3(rightArm.eulerAngles.x, rightArm.eulerAngles.y, - characterAngleB + ((Time.deltaTime * 100)));
                    }
                    
                    root.GetComponent<RotationConstraint>().rotationOffset = a;
                    transform.GetChild(0).transform.localRotation = Quaternion.Euler(b);
                    leftArm.localRotation = Quaternion.Euler(c);
                    rightArm.localRotation = Quaternion.Euler(d);


                }
                oldAngle = characterAngleA;
            }
        }
        else if(wasIdling)
        {
            wasIdling = false;
            Debug.Log("hey");
            StopAllCoroutines();
            StartCoroutine(SmoothReset());
        }
        */

    }
    /*
    IEnumerator SmoothReset()
    {
        rootFloat = Mathf.Sign(root.GetComponent<RotationConstraint>().rotationOffset.z);
        container = Mathf.Sign(transform.GetChild(0).transform.localRotation.eulerAngles.z);
        arms = Mathf.Sign(leftArm.localRotation.eulerAngles.z);

        while(root.GetComponent<RotationConstraint>().rotationOffset.z <= -2 || root.GetComponent<RotationConstraint>().rotationOffset.z >= 2)
        {
            characterAngleA = root.GetComponent<RotationConstraint>().rotationOffset.z;
            characterAngleB = transform.GetChild(0).transform.localRotation.eulerAngles.z;

            if (facingDirection() == -1)
            {
                root.GetComponent<RotationConstraint>().rotationOffset = root.GetComponent<RotationConstraint>().rotationOffset + (new Vector3(0, 0, -rootFloat * (Time.deltaTime * 200)));
                transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, transform.GetChild(0).transform.localRotation.eulerAngles.z - container * (Time.deltaTime * 200)));
                leftArm.localRotation = Quaternion.Euler(new Vector3(0, 0, leftArm.localRotation.eulerAngles.z - arms * (Time.deltaTime * 200)));
                rightArm.localRotation = Quaternion.Euler(new Vector3(0, 0, leftArm.localRotation.eulerAngles.z - arms * (Time.deltaTime * 200)));
            }
            else
            {
                root.GetComponent<RotationConstraint>().rotationOffset = (new Vector3(0, 0, characterAngleA + (Time.deltaTime * 200)));
                transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, characterAngleB - (Time.deltaTime * 200)));
                leftArm.localRotation = Quaternion.Euler(new Vector3(0, 0, -characterAngleB + (Time.deltaTime * 200)));
                rightArm.localRotation = Quaternion.Euler(new Vector3(0, 0, -characterAngleB +  (Time.deltaTime * 200)));
            }          
            yield return new WaitForEndOfFrame();
        }

        HardReset();
    }

    void HardReset()
    {
        root.GetComponent<RotationConstraint>().rotationOffset = new Vector3(0, 0, 0);
        root.GetComponent<RotationConstraint>().rotationAtRest = new Vector3(0, 0, 90);
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        leftArm.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        rightArm.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    */
    public void newMove(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (velocity.y <= 0)
        {
            DescendSlope(ref velocity);
        }
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

    //Places the edges of the groundCheck player collider depending on it's actual position in the world
    //Used to cast rays from and between these positions to detect collisions
    void UpdateRaycastOrigins()
    {
        Bounds bounds = groundCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //Order rays on the groundCHeck player collider depending on the number of rays
    void CalculateRaySpacing()
    {
        Bounds bounds = groundCollider.bounds;
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

    //Used to detect horizontal collisions
    public void HorizontalCollisions(ref Vector3 velocity)
    {
        float DirectionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (int i = 0; i < HorizontalRayCount; i++)
        {
            rayOrigin = (DirectionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * DirectionX, rayLength, m_WhatIsGround | m_DefaultLayer);
            Debug.DrawRay(rayOrigin, Vector2.right * DirectionX * rayLength, Color.red);
            if (hit)
            {
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                SignedSlopeAngle = Vector2.SignedAngle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    ClimbSlope(ref velocity, slopeAngle);
                }
                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = Mathf.Min(Mathf.Abs(velocity.x), (hit.distance - skinWidth)) * DirectionX;
                    rayLength = Mathf.Min(Mathf.Abs(velocity.x) + skinWidth, hit.distance);

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = DirectionX == -1;
                    collisions.right = DirectionX == 1;
                }
                
                
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocity = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y <= climbVelocity)
        {
            velocity.y = climbVelocity;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.SignedSlopeAngle = SignedSlopeAngle;
        }

    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        if (velocity.x == 0)
        {
            directionX = facingDirection();
        }
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, m_WhatIsGround);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            float SignedSlopeAngle = Vector2.SignedAngle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.SignedSlopeAngle = SignedSlopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    //Used to detect vertical collisions
    public void VerticalCollisions(ref Vector3 velocity)
    {
        float DirectionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (DirectionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * DirectionY, rayLength, m_WhatIsGround | m_DefaultLayer);
            Debug.DrawRay(rayOrigin, Vector2.up * DirectionY * rayLength, Color.red);
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * DirectionY;
                rayLength = hit.distance;

                collisions.below = DirectionY == -1;
                collisions.above = DirectionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, m_WhatIsGround);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                float SignedSlopeAngle = Vector2.SignedAngle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.SignedSlopeAngle = SignedSlopeAngle;
                }
            }
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

    public void Turn(float direction)
    {

        if (!GetComponent<CharacterMovement>().ableToMove)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl && !GetComponent<CharacterMovement>().ableToMove)
            {
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
        }
    }

    //Flips the player model
    private void Flip()
    {
        if (!GetComponent<CharacterMovement>().ableToMove)
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.GetChild(0).transform.localScale;
            theScale.x *= -1;
            transform.GetChild(0).transform.localScale = theScale;
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

    public bool isStoped()
    {
        return m_Rigidbody2D.velocity == Vector2.zero;
    }



    //Reset Jump values, function called on Landing
    public void ResetJump()
    {
        m_Grounded = true;
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
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope, descendingSlope;
        public float slopeAngle, OldSlopeAngle;
        public float SignedSlopeAngle,OldSignedSlopeAngle;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            OldSlopeAngle = slopeAngle;
            OldSignedSlopeAngle = SignedSlopeAngle;
            slopeAngle = 0;
        }
    }

    
}
