using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public bool canReceiveInput = true;                 // For determining if the character is able to receive an input 
    public bool inputReceived;                          // Checks if an attackInput has been pressed down
    private CharacterMovement CM;                       
    private CharacterController2D CC2d;
    [SerializeField] private float AttackMoveForce;


    public static CombatManager CMInstance;

    void Awake()
    {
        CMInstance = this;
        CM = GetComponent<CharacterMovement>();
        CC2d = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    //Checks if the player has pushed attack button
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !CM.isJumping() && !CM.player.GM.isPaused)
        {
            if (canReceiveInput)
            {
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

    //Function called on the animation to determines the ennemies who'll get hit by the attack
    public void CheckHit()
    {
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(GetComponent<Player>().attackPoint.position, GetComponent<Player>().attackRange);
        foreach (Collider2D actor in hitActors)
        {
            if (!actor.isTrigger && (actor.CompareTag("Ennemy") || actor.CompareTag("Destructible Wall")))
            {
                AddHeat();
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
