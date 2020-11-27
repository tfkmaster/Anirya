﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Actor
{
    public GameObject player;
    public float WanderSpeed = 3;
    public float FollowSpeed = 5;

    bool playerAlreadySelected = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!playerAlreadySelected)
        {
            CheckAttack();
        } 
    }

    void CheckAttack()
    {
        
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(attackPoint.position,attackRange);

        foreach(Collider2D actor in hitActors)
        {
            if (actor.CompareTag("Player") && !playerAlreadySelected)
            {
                Debug.Log("a");
                GetComponent<Animator>().SetTrigger("StartAttack");
            }
        }
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter,damages);
        Debug.Log("Ennemy hit");
    }

    public void DealDamages()
    {
        bool playerAlreadyDamaged = false;
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D actor in hitActors)
        {
            if (actor.CompareTag("Player") && !playerAlreadyDamaged)
            {
                actor.GetComponent<Player>().OnHit(this.gameObject, damageDone);
                playerAlreadyDamaged = true;
            }
        }
    }

    public void AttackStarted()
    {
        playerAlreadySelected = true;
    }

    public void AttackEnded()
    {
        playerAlreadySelected = false;
    }
}