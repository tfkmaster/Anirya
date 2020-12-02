using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private Vector2 knockBackForce = new Vector2(1, 1);     // Determines how much the player get's knocked back on being hit
    private Animator animator;                                              // The animator attached on the player
    private GameManager GM;

    [SerializeField] private float InactiveTime = 1f;                       //Determines the duration during which the player can't control the character
    private float InactiveCounter;                                          //Counts the time during which the character is inactive
    public bool isDead = false;                                             //Determines if the player is actually dead or not

    private bool isTriggeringInteractible;                                  //Set to true when the player is triggering an interactible entity
    private Interactible lastInteractible;                                  //Last interactible entity triggered by the player - set to null if the player is not interacting anymore        

    public Collider2D PlayerBox;
    public bool isOnOneWayPlatform = false;

    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //GM.Player = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = GM.LastCheckpoint.GetComponent<CheckpointController>().MyPosition.position;
        InactiveCounter = InactiveTime;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactible"))
        {
            isTriggeringInteractible = true;
            lastInteractible = collision.gameObject.GetComponent<Interactible>();
        }
        if (collision.gameObject.CompareTag("One Way Platform"))
        {
            isOnOneWayPlatform = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactible"))
        {
            isTriggeringInteractible = false;
            lastInteractible = null;
        }
        if (collision.gameObject.CompareTag("One Way Platform"))
        {
            isOnOneWayPlatform = false;
            collision.gameObject.GetComponent<OneWayPlatform>().setBackEffector();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Character inactive duration
        if (GetComponent<CharacterMovement>().Inactive == true)
        {
            InactiveCounter -= Time.deltaTime;
        }
        if (InactiveCounter <= 0)
        {
            GetComponent<CharacterMovement>().Inactive = false;
            InactiveCounter = InactiveTime;
        }

        //Interactible objects triggered one time by the player input
        if (Input.GetKeyDown(KeyCode.E) && isTriggeringInteractible)
        {
            lastInteractible.Trigger();
        }

        //Resume or Pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GM.SetPause();
        }
    }

    //Function called when the actor gets hit
    public override void OnHit(GameObject hitter, int damages)
    {
        if (!GetComponent<CharacterMovement>().Inactive)
        {
            GetComponent<CharacterMovement>().Inactive = true;
            GetComponent<CharacterController2D>().stopCharacter();
            if (hitter.GetComponent<Transform>().position.x > GetComponent<Transform>().position.x)
            {
                GetComponent<Rigidbody2D>().AddForce(knockBackForce * new Vector2(-1, 1), ForceMode2D.Impulse);
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(knockBackForce, ForceMode2D.Impulse);
            }
            base.OnHit(hitter, damages);
        }
    }

    // Function called when the player dies
    protected override void Death()
    {
        base.Death();
        isDead = true;
        animator.SetBool("dead", true);
    }

    public void LoadCheckpoint()
    {
        GM.LoadCheckpoint();
        animator.SetBool("dead", false);
    }

    public void Reborn()
    {
        Debug.Log("reborn");
        isDead = false;
        healthPoints = maxHealthPoints;
    }
}
