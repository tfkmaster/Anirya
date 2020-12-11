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
    [SerializeField] private GameObject leftHandFire;
    [SerializeField] private GameObject rightHandFire;


    private Rigidbody2D m_Rigidbody2D;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    
    //Movement Information
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float maxInternSpeed; //Value indicating the max speed the player can reach in his actual state

    //Jump Informations
    private bool topReached = false; // For determining if the jump climax has been reached
    private bool forceDescent = false; // For determining if the descent of the player after a jump must be forced or not, depending if he reached his jump climax
    private float yPos; //Y pos the player was on the base of his jump
    private bool yPosRemembered = false; //For determining if the Y value on the base of the jump has been already stored  

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
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
        if (collision.gameObject.layer == 9 && !GetComponent<Player>().isOnOneWayPlatform && !collision.otherCollider.gameObject.CompareTag("One Way Platform") /*&& m_Rigidbody2D.velocity.y <= 0f*/ && m_Grounded)
        {
            OnLandEvent.Invoke();
            //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -0.5f);
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
            if (m_Grounded || m_AirControl)
            {
                m_Rigidbody2D.velocity = new Vector2(direction * accelerationX / 10, m_Rigidbody2D.velocity.y);

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
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Vector2.up.y * m_JumpForce);
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

    public void EnableLeftFlame()
    {
        leftHandFire.SetActive(true);
    }
    public void EnableRightFlame()
    {
        rightHandFire.SetActive(true);
    }
    public void DisableLeftFlame()
    {
        leftHandFire.SetActive(false);
    }

    public void DisableRightFlame()
    {
        rightHandFire.SetActive(false);
    }
}
