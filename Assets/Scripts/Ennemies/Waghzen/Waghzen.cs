using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waghzen : Ennemy
{
    public float waitTime;

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

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
        if (!dead)
        {
            //StartCoroutine(Knockback());
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube((transform.position + attackPoint.position) / 2, new Vector3(Vector3.Distance(transform.position, attackPoint.position), attackRange, 0));
    }

}
