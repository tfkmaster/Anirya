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

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && !CM.isJumping())
        {
            if (canReceiveInput)
            {
                CC2d.getRigidbody().AddForce(new Vector2(CC2d.facingDirection() * AttackMoveForce, 0), ForceMode2D.Impulse);
                canReceiveInput = false;
                inputReceived = true;
            }
            else
            {
                return;
            }
        }
    }

    public void CheckHit()
    {
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(GetComponent<Player>().attackPoint.position, GetComponent<Player>().attackRange);
        foreach (Collider2D actor in hitActors)
        {
            if (actor.CompareTag("Ennemy"))
            {
                actor.GetComponent<Actor>().OnHit(gameObject, GetComponent<Player>().GetDamageDone());
            }
        }
    }
}
