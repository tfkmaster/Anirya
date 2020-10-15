using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected int HealthPoints;
    protected int damageDone;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnHit(GameObject hitter, int damages)
    {
        if((HealthPoints-damages) < 0)
        {
            HealthPoints = 0;
            Death();
        }
        else
        {
            HealthPoints -= damages;
        }
    }

    void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    protected virtual void Death()
    {

    }
}
