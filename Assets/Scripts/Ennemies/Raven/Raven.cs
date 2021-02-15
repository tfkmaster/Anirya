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
    [Tooltip("Name WanderPoint following \"w00\" + WanderIndex")]
    public List<Transform> WanderPoints;
    public Transform MoveTo;

    [Header("Collision Management")]
    [Tooltip("Time during which raven is stunned and lose focus")]
    public float CollidingTime = 0.5f;
    public float MagnitudeMax = 5.0f;
    private float collidingTimer;
    private bool colliding = false;
    private int wanderIndex = 0;

    void Start()
    {
        //initWanderPoints();

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        collidingTimer = CollidingTime;

        MoveTo = WanderPoints[wanderIndex];       
    }

    void Update()
    {
        
    }

    void initWanderPoints()
    {
        int WanderPointCount = transform.GetChild(0).childCount;
        for (int i = 0; i < WanderPointCount; ++i)
        {
            WanderPoints.Add(transform.GetChild(0).GetChild(i));
        }
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wander Point") && animator.GetBool("IsWandering"))
        {
            if(collision.transform.name == "w00" + (wanderIndex % WanderPoints.Count + 1).ToString())
            {
                MoveTo = WanderPoints[++wanderIndex % WanderPoints.Count];
            }
        }
    }

    public void FlipSprite()
    {
        if(transform.position.x - MoveTo.position.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
