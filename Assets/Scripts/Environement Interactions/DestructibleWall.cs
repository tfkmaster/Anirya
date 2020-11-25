using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : Actor
{
    protected override void Death()
    {
        GetComponent<Animator>().SetBool("dead", true);
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
        GetComponent<Animator>().SetTrigger("hit");
        Debug.Log("Wall hit");
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
