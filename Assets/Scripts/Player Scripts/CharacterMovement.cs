using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Linked components
    private CharacterController2D cc2d; //The Movement controller attached on player
    private Animator animator; // The animator attached on the player

    private int direction = 0; //Value indicating whether the player is moving to the right, to the left or is just static
    [SerializeField] private float acceleration;

    private float horizontalMove = 0.0f;
    private float lastValue;
    private float controllerHorizontalValue = 0.0f;

    private bool jump = false;

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

    }

    void FixedUpdate()
    {
        cc2d.Move(horizontalMove, jump);
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

    void horizontalMoveResult()
    {
        horizontalMove = direction;
    }

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
        if (Input.GetButtonDown("Jump") && !jump)
        {
            jump = true;
            animator.SetBool("jump", true);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("jump", false);
        cc2d.ResetJump();
        jump = false;
    }

    //jump getter
    public bool isJumping()
    {
        return jump;
    }

    //direction getter
    public int getDirection()
    {
        return direction;
    }
}