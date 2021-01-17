﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Ennemy
{
    public bool knockbacked = false;
    public bool playerGrounded = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //StartFight();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.otherCollider.GetType() == typeof(BoxCollider2D) && collision.collider.gameObject.CompareTag("Player") && collision.collider.isTrigger)
        {
            playerGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider.GetType() == typeof(BoxCollider2D) && collision.collider.gameObject.CompareTag("Player") && collision.collider.isTrigger)
        {
            playerGrounded = false;
        }
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
        if (!dead)
        {
            StartCoroutine(Knockback());
        }
    }

    protected override void Death()
    {
        base.Death();
        GetComponentInChildren<Animator>().SetBool("dead", true);
    }

    public void StartFight()
    {
        GetComponentInChildren<Animator>().SetBool("startFight", true);
    }

    private IEnumerator Knockback()
    {
        knockbacked = true;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        knockbacked = false;

    }

    public IEnumerator WolfDash()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(30* (GetComponent<AIWolf>().m_FacingRight ? 1 : -1), 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    }

}
