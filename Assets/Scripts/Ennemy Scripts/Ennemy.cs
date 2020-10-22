using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Actor
{


    // Start is called before the first frame update
    void Start()
    {
        
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
