using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody2D))]
public class Raven : Actor
{
    [Header("Raven Components")]
    public Rigidbody2D rb2D;
    public Animator animator;

    [Header("Raven Datas")]
    public float thrust = 6.0f;
    public Player player;

    [Header("Wander")]
    public WanderManager WanderManager;
    public Transform MoveTo;

    [Header("Collision Management")]
    [Tooltip("Time during which raven is stunned and lose focus")]
    public float CollidingTime = 0.5f;
    public float MagnitudeMax = 5.0f;
    public bool m_FacingRight = false;
    private float collidingTimer;
    private bool colliding = false;
    private int wanderIndex = 0;
    

    void Start()
    {        
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        collidingTimer = CollidingTime;

        MoveTo = WanderManager.GetWanderPoints()[wanderIndex];       
    }

    public bool IsPlayerOnSight()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                var ennemyMask = (1 << 13);
                ennemyMask = ~ennemyMask;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - col.transform.position), ennemyMask);
                
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    return true;
                }
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        Gizmos.DrawRay(transform.position, direction);
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
        if (animator.GetBool("IsAttacking"))
        {
            rb2D.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wander Point") && animator.GetBool("IsWandering"))
        {
            if(collision.transform.name == "w00" + (wanderIndex % WanderManager.GetWanderPoints().Count + 1).ToString())
            {
                MoveTo = WanderManager.GetWanderPoints()[++wanderIndex % WanderManager.GetWanderPoints().Count];
            }
        }
    }

    public void FlipSprite()
    {

        if (transform.position.x - MoveTo.position.x > 0 && !m_FacingRight)
        {
            m_FacingRight = true;
            Vector3 theScale = transform.transform.localScale;
            theScale.x *= -1;
            transform.transform.localScale = theScale;
        }
        else if(transform.position.x - MoveTo.position.x < 0 && m_FacingRight)
        {
            m_FacingRight = false;
            Vector3 theScale = transform.transform.localScale;
            theScale.x *= -1;
            transform.transform.localScale = theScale;
        }
    }




    // attribute getters
    public bool IsColliding()
    {
        return colliding;
    }

    public float GetCollidingTimer()
    {
        return collidingTimer;
    }

    public float GetMagnitudeMax()
    {
        return MagnitudeMax;
    }

    // attribute setters
    public void SetColliding(bool isColliding)
    {
        colliding = isColliding;
    }

    public void SetCollidingTimer(float col)
    {
        collidingTimer = col;
    }

    public void ResetCollidingTimer()
    {
        collidingTimer = CollidingTime;
    }
}
