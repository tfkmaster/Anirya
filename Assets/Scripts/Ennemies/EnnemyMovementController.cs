using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovementController : MonoBehaviour
{
    //Movement Information
    protected bool m_FacingRight = true;  // For determining which way the ennemy is currently facing.
    public float nearPlayerStop = 1f; //Determines at what distances the wolf stop following the player when he is near him, so that he doesn't spam left and right movement to match the exact player's position

    public Collider2D MovementZone; // Determines the zone where the ennemy can follow the player
    public GameObject player;

    public bool playerOnSight = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Flip();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If the ennemy is facing left and moves to the right...
        if (GetComponent<Rigidbody2D>() && GetComponent<Rigidbody2D>().velocity.x > 0.5f && !m_FacingRight && !GetComponent<Wolf>().knockbacked)
        {
            // ... flip the ennemy.
            Flip();
        }
        // Otherwise if the ennemy is facing right and moves to the left...
        else if (GetComponent<Rigidbody2D>() && GetComponent<Rigidbody2D>().velocity.x < -0.5f && m_FacingRight && !GetComponent<Wolf>().knockbacked)
        {
            // ... flip the ennemy.
            Flip();
        }
        if (playerOnSight && player.GetComponent<Collider2D>().IsTouching(MovementZone) && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Wander"))
        {
            GetComponentInChildren<Animator>().SetTrigger("StartFollowing");
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!player)
            {
                player = col.gameObject;
            }
            playerOnSight = true;
            if (player.GetComponent<Collider2D>().IsTouching(MovementZone))
            {
                GetComponentInChildren<Animator>().SetTrigger("StartFollowing");
            }
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerOnSight = true;
        }
    }

        protected void OnTriggerExit2D(Collider2D col)
    {
        if(col == MovementZone)
        {
            GetComponentInChildren<Animator>().SetTrigger("StartWandering");
        }
        if (col.gameObject.CompareTag("Player"))
        {
            playerOnSight = false;
        }
    }

    //Flips the player model
    protected void Flip()
    {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.GetChild(0).transform.localScale;
            theScale.x *= -1;
            transform.GetChild(0).transform.localScale = theScale;
    }
}
