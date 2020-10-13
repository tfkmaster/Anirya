using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Header("- Jump Settings -")]
    [SerializeField] private float m_JumpForce = 150f;                          // Amount of force added when the player jumps.
    [SerializeField] private bool m_AirControl = true;                          // Whether or not a player can steer while jumping;
    [SerializeField] private float jumpHigh = 4;                                // How high the character can jump
    [Range(0, 1)] [SerializeField] private float clampingValueY = 0.9f;         // Percentage of clamping the y velocity value

    [Header("- Max Player Speed Settings -")]
    [SerializeField] private float maxHorizontalSpeedOnGround = 10f;            // The highest velocity on the X axis the character can go while grounded
    [SerializeField] private float maxHorizontalSpeedOnAir = 10f;               // The highest velocity on the X axis the character can go while onAir
    [SerializeField] private float maxJumpSpeedGoingUp = 12f;                   // The highest velocity on the Y axis the character can go while going upward during jump
    [SerializeField] private float maxJumpSpeedGoingDown = 11f;                 // The highest velocity on the Y axis the character can go while going downward during jump

    [Header("- Move Settings -")]
    [SerializeField] private float accelerationX = 0.1f;                        // The value of the acceleration to reach maxSpeed
    [Range(0, 1)] [SerializeField] private float clampingValueX = 0.9f;         // Percentage of clamping the x velocity value

    [Header("- EnvironmentInfo Settings -")]
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

    public List<Collider2D> PlayerColliders;

    private Rigidbody2D m_Rigidbody2D;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    
    //Movement Information
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float maxInternSpeed; //Value indicating the max speed the player can reach in his actual state

    //Jump Informations
    private bool m_Grounded;            // Whether or not the player is grounded.
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
        if (Input.GetButtonUp("Jump") && !topReached)
        {
            topReached = true;
            forceDescent = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            m_Grounded = true;
            OnLandEvent.Invoke();
        }
    }

    public void Move(float direction, bool jump)
    {

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            //Change the character max speed depending if onAir or not
            if (jump)
            {
                maxInternSpeed = maxHorizontalSpeedOnAir;
            }
            else
            {
                maxInternSpeed = maxHorizontalSpeedOnGround;
            }


            if (Mathf.Abs(m_Rigidbody2D.velocity.x) <= maxInternSpeed)
            {
                m_Rigidbody2D.AddRelativeForce(new Vector2(direction * accelerationX, 0));
            }
            else
            {
                m_Rigidbody2D.velocity *= new Vector2(clampingValueX,1);
            }

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
        // If the player should jump...
        
        if (!topReached && jump && Input.GetButton("Jump"))
        {
            //Remember the Y Pos on the base of the jump
            if (!yPosRemembered)
            {
                yPos = GetComponent<Transform>().position.y;
                yPosRemembered = true;
            }
            
            // Add a vertical force to the player and maintains it while the jumpClimax Hasn't been reached.
            m_Grounded = false;
            if(GetComponent<Transform>().position.y <= (yPos + jumpHigh))
            {
                if (Mathf.Abs(m_Rigidbody2D.velocity.y) <= maxJumpSpeedGoingUp)
                {
                    m_Rigidbody2D.AddRelativeForce(new Vector2(0f, m_JumpForce));
                }
                else
                {
                    m_Rigidbody2D.velocity *= new Vector2(1, clampingValueY);
                }
                
            }
            else
            {
                topReached = true;
                yPosRemembered = false;
                return;
            }
        }
        //If the player release the jump before the climax, force the character descent
        if (forceDescent)
        {
            if (-Mathf.Abs(m_Rigidbody2D.velocity.y) >= -maxJumpSpeedGoingDown)
            {
                m_Rigidbody2D.AddRelativeForce(new Vector2(0f, -m_JumpForce));
            }
            else
            {
                m_Rigidbody2D.velocity *= new Vector2(1, clampingValueY);
            }
        }
        
    }

    //Flips the player model
    private void Flip()
    {
        stopCharacter();
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

        //Getter that returns the character rigidbody2D
        public Rigidbody2D getRigidbody()
    {
        return m_Rigidbody2D;
    }

    //Setter stoping the player from moving instantly
    public void stopCharacter()
    {
        m_Rigidbody2D.velocity = new Vector3(0, m_Rigidbody2D.velocity.y, 0);
        m_Rigidbody2D.angularVelocity = 0;
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

    //Reset Jump values, function called on Landing
    public void ResetJump()
    {
        topReached = false;
        forceDescent = false;
    }



    public void GetColliderInfos(Collider2D col)
    {

        GetComponent<Collider2D>().offset = col.offset;
        //GetComponent<Collider2D>().size = col.size;
    }

}
