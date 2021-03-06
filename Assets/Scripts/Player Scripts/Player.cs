﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private Vector2 knockBackForce = new Vector2(1,1);     // Determines how much the player get's knocked back on being hit
    private Animator animator;                                              // The animator attached on the player

    [SerializeField] private float InactiveTime = 1f;                       //Determines the duration during which the player can't control the character
    private float InactiveCounter;                                          //Counts the time during which the character is inactive
    public bool isDead = false;                                             //Determines if the player is actually dead or not

    // Start is called before the first frame update
    void Start()
    {
        InactiveCounter = InactiveTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Character inactive duration
        if(GetComponent<CharacterMovement>().Inactive == true)
        {
            InactiveCounter -= Time.deltaTime;
        }
        if(InactiveCounter <= 0)
        {
            GetComponent<CharacterMovement>().Inactive = false;
            InactiveCounter = InactiveTime;
        }
    }

    //Function called when the actor gets hit
    public override void OnHit(GameObject hitter,  int damages)
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
}
