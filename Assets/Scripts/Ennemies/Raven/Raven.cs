using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Rigidbody2D))]
public class Raven : MonoBehaviour
{
    public List<Transform> WanderPoints;
    public float speed = 3.0f;
    public Transform MoveTo;
    private int wanderIndex = 0;
    public float thrust = 24f;

    private Rigidbody2D rb2D;
    public const float collidingTime = 0.5f;
    public float MagnitudeMax = 5f;
    private float collidingTimer = collidingTime;
    private bool colliding = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        MoveTo = WanderPoints[wanderIndex];       
    }

    void Update()
    {
        if (colliding)
        {
            collidingTimer -= Time.deltaTime;
        }
        if(collidingTimer <= 0f)
        {
            collidingTimer = collidingTime;
            colliding = false;
        }

        if(rb2D.velocity.magnitude > MagnitudeMax)
        {
            rb2D.velocity *= 1 - ((rb2D.velocity.magnitude / MagnitudeMax) - 1);
        }

    }

    void FixedUpdate()
    {       
        if(!colliding)
        {
            Vector3 direction = MoveTo.position - transform.position;
            rb2D.AddForce(direction.normalized * thrust, ForceMode2D.Force);
        }

        FlipRaven();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wander Point"))
        {
            if(collision.transform.name == "w00" + (wanderIndex % WanderPoints.Count + 1).ToString())
            {
                MoveTo = WanderPoints[++wanderIndex % WanderPoints.Count];
            }
        }
    }

    void FlipRaven()
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
}
