using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Actor
{
    public GameObject player;
    public float WanderSpeed = 3;
    public float FollowSpeed = 5;

    public Collider2D MovementZone;
    public Collider2D AggroDistance;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
            Attack();
    }

    void Attack()
    {
        Collider2D[] hitActors = Physics2D.OverlapCircleAll(attackPoint.position,attackRange);

        foreach(Collider2D actor in hitActors)
        {
            if (actor.CompareTag("Player"))
            {
                actor.GetComponent<Player>().OnHit(this.gameObject, damageDone);
            }
        }
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter,damages);
        Debug.Log("Ennemy hit");
    }
}
