using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : Actor
{
    protected override void Death()
    {
        Destroy(this.gameObject);
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
        Debug.Log("Wall hit");
    }
}
