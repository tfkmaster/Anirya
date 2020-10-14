using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Linked components
    private CharacterController2D cc2d;             // The Movement controller attached on player
    private Animator animator;                      // The animator attached on the player

    private int direction = 0;                      // Value indicating whether the player is moving to the right, to the left or is just static
    [SerializeField] private float acceleration;    // Controller movement accelleration (how fast the character will reach the maximum speed

    //Keyboard Informations
    private float horizontalMove = 0.0f;            // Used to give him speed in that direction (Keyboard) 

    //Controller Informations
    private float lastValue;                        // To Check when the player flips the character (Controller)
    private float controllerHorizontalValue = 0.0f; // Used to give him speed in that direction (Controller) 


    [Header("- Coyote Time Settings -")]
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeAfterLeavingGround = 0.1f;      //CoyoteTime after Leaving the ground
    [Range(0.001f, 1)] [SerializeField] private float coyoteTimeBeforeReachingGround = 0.1f;    //CoyoteTime before reaching the ground

    //Coyote Time Counters
    private float groundedTimeCount = 0.0f;         // Counts the last time the character was in contact with the floor
    private float jumpPressedTimeCount = 0.0f;      // Counts the last time the player pushed the jump button

    //Jump Information
    private bool onAir = false;                     // Determines if the player is currently in the air or not
    private bool jumpValidation = false;            // Determines if the player is allowed to jump in his current situation

    // Awake is called once before Start
    void Awake()
    {
        cc2d = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardKeyDetection();
        Actions();
        applyMovement();
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

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

    void FixedUpdate()
    {
        //Move the character
        cc2d.Move(horizontalMove, onAir);
    }

    //Detects which keyboard movement keys are actually pressed and returns the value corresponding
    // -1 for left / 1 for right / 0 for static
    void KeyboardKeyDetection()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            direction = 0;
            cc2d.stopCharacter();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 0;
            cc2d.stopCharacter();
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 0;
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

        if (controllerHorizontalValue < 0.3f && lastValue >= 0.3f
            || controllerHorizontalValue > -0.3f && lastValue <= -0.3f)
        {
            cc2d.stopCharacter();
        }
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
        if ((Input.GetButtonDown("Jump") && !onAir && jumpValidation) || (Input.GetButton("Jump") && cc2d.getGrounded() && jumpValidation) || (Input.GetButton("Jump") && !onAir && jumpValidation))
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
}