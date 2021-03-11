using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //Stats
    [SerializeField] public int maxHealthPoints;
    [SerializeField] public int healthPoints;
    [SerializeField] public int damageDone;
    protected bool dead = false;

    //Attack
    public Transform attackPoint;
    public Transform attackPoint2;
    public float attackRange = 0.5f;

    //Damages visual Feedback
    private bool gotHit = false;
    [SerializeField] private float timeRed = 0.5f;
    private float timeRedCounter = 0;

    //Itij essences management
    public int itijEssences = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!gameObject.CompareTag("Player"))
        {
            if (gotHit)
            {
                timeRedCounter += Time.deltaTime;
            }
            if (timeRedCounter >= timeRed)
            {
                gotHit = false;
                timeRedCounter = 0;
                SpriteRenderer[] spr_renderers = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer spr in spr_renderers)
                {
                    spr.color = new Color(1, 1, 1);
                }
            }
        }
    }

    //Function called when the actor gets hit
    public virtual void OnHit(GameObject hitter, int damages)
    {
        if (!dead && !gameObject.CompareTag("Player"))
        {
            DamageFeedback();
        }

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
        Gizmos.DrawCube((attackPoint.position + attackPoint2.position) / 2, new Vector3(Vector3.Distance(attackPoint.position,attackPoint2.position), attackRange, 0));

    }

    protected virtual void Death()
    {
        if (gameObject.CompareTag("Ennemy"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().itijEssencesCarried += itijEssences;
        }
        gameObject.layer = LayerMask.NameToLayer("DeadActor");
        dead = true;
    }

    //Displays a visual feedback on the current Actor
    protected virtual void DamageFeedback()
    {
        SpriteRenderer[] spr_renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spr in spr_renderers)
        {
            spr.color = new Color(1, 0, 0);
            timeRedCounter = 0;
        }
        gotHit = true;
    }

    public int GetDamageDone()
    {
        return damageDone;
    }

    public bool GetDead()
    {
        return dead;
    }
}
