using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //Stats
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int healthPoints;
    [SerializeField] protected int damageDone;
    
    //Attack
    public Transform attackPoint;
    public float attackRange = 0.5f;

    //Damages visual Feedback
    private bool gotHit = false;
    [SerializeField] private float timeRed = 0.5f;
    private float timeRedCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (gotHit)
        {
            timeRedCounter += Time.deltaTime;
        }
        if (timeRedCounter >= timeRed)
        {
            gotHit = false;
            timeRedCounter = 0;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    //Function called when the actor gets hit
    public virtual void OnHit(GameObject hitter, int damages)
    {

        DamageFeedback();

        if ((healthPoints-damages) <= 0)
        {
            healthPoints = 0;
            Death();
        }
        else
        {
            healthPoints -= damages;
        }
    }

    //Visualize The attack range
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

    //Displays a visual feedback on the current Actor
    protected virtual void DamageFeedback()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        timeRedCounter = 0;
        gotHit = true;
    }

    public int GetDamageDone()
    {
        return damageDone;
    }
}
