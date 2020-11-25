using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : Actor
{
    public ParticleSystem onHitParticles1;
    public ParticleSystem onHitParticles2;
    protected override void Death()
    {
        Destroy(GetComponent<BoxCollider2D>());
        GetComponent<Animator>().SetBool("dead", true);
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        onHitParticles1.Play();
        onHitParticles2.Play();
        base.OnHit(hitter, damages);
        GetComponent<Animator>().SetTrigger("hit");
        Debug.Log("Wall hit");
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
