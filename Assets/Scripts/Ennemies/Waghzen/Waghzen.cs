using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waghzen : Ennemy
{
    public float waitTime;
    public Transform attackPoint3;

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
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((transform.position + attackPoint.position) / 2, new Vector3(Vector3.Distance(transform.position, attackPoint.position), attackRange, 0));
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube((attackPoint2.position + attackPoint3.position) / 2, new Vector3(Vector3.Distance(attackPoint2.position, attackPoint3.position), attackRange, 0));
    }
}
