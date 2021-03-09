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
    private float attackRangeMult = 1f;
    public float wanderThrustMult = 5f;

    public bool launchSting = false;
    public bool freeRaven = false;

    public int RavenMask = ~(1 << 13);


    void Start()
    {        
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        collidingTimer = CollidingTime;

        MoveTo = WanderManager.GetWanderPoints()[wanderIndex];       
    }

    void Update()
    {
        applyLinearDrag();
    }

    public bool IsPlayerOnSight()
    {
        if (animator.GetBool("IsWandering"))
        {
            attackRangeMult = 1f;
        }
        else
        {
            attackRangeMult = 3f;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange * attackRangeMult);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (col.transform.position - transform.position), 100000f, RavenMask);
                
                if (hit.collider.CompareTag("Player"))
                {
                    //hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    return true;
                }
                else
                {
                    Debug.Log(hit.collider.name);
                }
            }
        }

        //MoveTo.GetComponent<SpriteRenderer>().color = Color.green;
        return false;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange *attackRangeMult);

        Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        Gizmos.DrawRay(transform.position, direction);
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.OnHit(gameObject, damageDone);
        }

        colliding = true;
    }

    void OnCollisionExit(Collision collision)
    {
        colliding = false;
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

    public void LaunchSting()
    {
        launchSting = true;
    }

    public void LeaveAttackState()
    {
        animator.SetBool("IsFollowing", true);
        animator.SetBool("IsAttacking", false);
    }

    void applyLinearDrag()
    {
        if (animator.GetBool("IsWandering"))
        {
            rb2D.drag = 1f;
        }
        else
        {
            rb2D.drag = 0f;
        }
    }
}
