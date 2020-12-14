using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public bool canReceiveInput = true;                 // For determining if the character is able to receive an input 
    public bool inputReceived;                          // Checks if an attackInput has been pressed down
    
    private bool regenButtonHeldDown = false;
    public float HoldRegenTime = 1.5f;
    private float holdRegenCounter = 0;

    private CharacterMovement CM;                       
    private CharacterController2D CC2d;
    private Player player;
    [SerializeField] private float AttackMoveForce;

    public ParticleSystem HitParticles;


    public static CombatManager CMInstance;

    void Awake()
    {
        CMInstance = this;
        CM = GetComponent<CharacterMovement>();
        CC2d = GetComponent<CharacterController2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CM.player.GM.isPaused)
        {
            Attack();
            Regen();
        }
        
    }

    //Checks if the player has pushed attack button
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !CM.isJumping())
        {
            if (canReceiveInput)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                CM.canMove = false;
                canReceiveInput = false;
                inputReceived = true;
                CC2d.getRigidbody().AddForce(new Vector2(CC2d.facingDirection() * AttackMoveForce, 0), ForceMode2D.Impulse);
            }
            else
            {
                return;
            }
        }
    }

    public void Regen()
    {
            if (Input.GetButtonDown("Fire2") && player.actualHeat >= player.RegenCost)
            {
                holdRegenCounter = 0;
                regenButtonHeldDown = true;
                CM.canMove = false;
                canReceiveInput = false;
                GetComponentInChildren<Animator>().SetTrigger("startRegen");
            }
            if (Input.GetButtonUp("Fire2"))
            {
                regenButtonHeldDown = false;
                CM.canMove = true;
                canReceiveInput = true;
                GetComponentInChildren<Animator>().SetTrigger("stopRegen");
            }

            if (regenButtonHeldDown && player.healthPoints != player.maxHealthPoints)
            {
                holdRegenCounter += Time.deltaTime;
                player.actualHeat -= Time.deltaTime * player.RegenCost / HoldRegenTime;
                if (holdRegenCounter >= HoldRegenTime)
                {
                    regenButtonHeldDown = false;
                    player.healthPoints += 1;
                    GetComponentInChildren<Animator>().SetTrigger("stopRegen");
                }
                player.SendPlayerStatsToGameManager();
            }
    }

    //Function called on the animation to determines the ennemies who'll get hit by the attack
    public void CheckHit()
    {
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(GetComponent<Player>().attackPoint.position, GetComponent<Player>().attackRange);
        foreach (Collider2D actor in hitActors)
        {
            if (!actor.isTrigger && (actor.CompareTag("Ennemy") || actor.CompareTag("Destructible Wall")))
            {
                AddHeat();
                Instantiate(HitParticles, GetComponent<Player>().attackPoint.position, new Quaternion());
                actor.GetComponent<Actor>().OnHit(gameObject, GetComponent<Player>().GetDamageDone());
            }
        }
    }

    public void AddHeat()
    {
        if (GetComponent<Player>().actualHeat + GetComponent<Player>().AddHeat <= 100)
        {
            GetComponent<Player>().actualHeat += GetComponent<Player>().AddHeat;
        }
        else
        {
            GetComponent<Player>().actualHeat = 100;
        }
        GetComponent<Player>().SendPlayerStatsToGameManager();
    }

}
