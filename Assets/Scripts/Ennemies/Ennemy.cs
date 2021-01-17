using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Actor
{
    public GameObject player;
    public float WanderSpeed = 3;
    public float FollowSpeed = 5;
    protected bool fading;

    public float disapearTime = 1f;
    private float disapearTimer = 0;

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
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!playerAlreadySelected)
        {
            CheckAttack();
        }
        if (fading)
        {
            AnimateFade();
        }
    }

    void CheckAttack()
    {

        Vector2 a = attackPoint.position;
        Vector2 b = attackPoint2.position;

        Collider2D[] hitActors = Physics2D.OverlapBoxAll((a + b) / 2, new Vector2(Vector3.Distance(attackPoint.position, attackPoint2.position), attackRange),0);

        foreach (Collider2D actor in hitActors)
        {
            if (actor.CompareTag("Player") && !playerAlreadySelected)
            {
                GetComponentInChildren<Animator>().SetTrigger("StartAttack");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && dead)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }

        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<Player>().isDead)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }


        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().OnHit(this.gameObject, damageDone);
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

    protected override void Death()
    {
        base.Death();
    }

    protected void AnimateFade()
    {
            SpriteRenderer[] spr_renderers = GetComponentsInChildren<SpriteRenderer>();
            if (disapearTimer < disapearTime)
            {
                disapearTimer += Time.deltaTime;
                foreach (SpriteRenderer spr in spr_renderers)
                {
                    spr.color = new Color(1, 1, 1, spr.color.a - Time.deltaTime / disapearTime);
                }
            }
            else
            {
                Destroy(gameObject);
            }
    }

    public void StartFade()
    {
        fading = true;
    }

}
