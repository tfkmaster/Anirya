using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private Vector2 knockBackForce = new Vector2(1, 1);     // Determines how much the player get's knocked back on being hit
    private Animator animator;                                              // The animator attached on the player
    public GameManager GM;

    [SerializeField] private float InputFreezeDuration = 1f;                       //Determines the duration during which the player can't control the character
    [SerializeField] private float knockbackVelocityX;
    [SerializeField] private float knockbackVelocityY;

    private float unableToMoveCounter;                                          //Counts the time during which the character is inactive
    public bool isDead = false;                                             //Determines if the player is actually dead or not

    private bool isTriggeringInteractible;                                  //Set to true when the player is triggering an interactible entity
    private Interactible lastInteractible;                                  //Last interactible entity triggered by the player - set to null if the player is not interacting anymore        

    public Collider2D PlayerBox;
    public bool isOnOneWayPlatform = false;

    public float maxHeat = 100;
    public float actualHeat;
    public float AddHeat = 15;
    public float RegenCost = 40;

    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //GM.Player = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = GM.LastCheckpoint.GetComponent<CheckpointController>().MyPosition.position;
        SendPlayerStatsToGameManager();
        unableToMoveCounter = InputFreezeDuration;
        animator = GetComponentInChildren<Animator>();
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
        if (GetComponent<CharacterMovement>().ableToMove == true)
        {
            unableToMoveCounter -= Time.deltaTime;
        }
        if (unableToMoveCounter <= 0)
        {
            GetComponent<CharacterMovement>().ableToMove = false;
            GetComponent<CharacterMovement>().StopKnockBack();
            unableToMoveCounter = InputFreezeDuration;
        }

        //Interactible objects triggered one time by the player input
        if (Input.GetKeyDown(KeyCode.E) && isTriggeringInteractible)
        {
            lastInteractible.Trigger();
        }

        //Resume or Pause the game
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7")) && !GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().controlScreenOn)
        {
            GM.SetPause();
        }
    }

    //Function called when the actor gets hit
    public override void OnHit(GameObject hitter, int damages)
    {
        if (!GetComponent<CharacterMovement>().ableToMove)
        {
            GetComponent<CharacterMovement>().ableToMove = true;
            Vector3 direction = (GetComponent<Transform>().position - hitter.GetComponent<Transform>().position).normalized;
            direction.x = Mathf.Sign(direction.x) * knockbackVelocityX;
            direction.y = knockbackVelocityY;
            GetComponent<CharacterMovement>().StartKnockBack(new Vector2(direction.x,direction.y));
            base.OnHit(hitter, damages);
            SendPlayerStatsToGameManager();
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

    public void SendPlayerStatsToGameManager()
    {
        GM.GetComponent<GameManager>().SendPlayerStatsToUIManager(maxHealthPoints, healthPoints, maxHeat, actualHeat);
    }
}
