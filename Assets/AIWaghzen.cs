using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaghzen : MonoBehaviour
{

    public float GravityAfterJumpValue;

    //Movement Information
    public bool m_FacingRight = true;  // For determining which way the ennemy is currently facing.
    public GameObject player;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Flip();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player.transform.position.x < gameObject.transform.position.x && m_FacingRight)
        {
            Flip();
        }

        if (player.transform.position.x > gameObject.transform.position.x && !m_FacingRight)
        {
            Flip();
        }

        // If the ennemy is facing left and moves to the right...
        if (GetComponentInParent<Rigidbody2D>() && GetComponentInParent<Rigidbody2D>().velocity.x > 0.5f && !m_FacingRight)
        {
            // ... flip the ennemy.
            Flip();
        }
        // Otherwise if the ennemy is facing right and moves to the left...
        else if (GetComponentInParent<Rigidbody2D>() && GetComponentInParent<Rigidbody2D>().velocity.x < -0.5f && m_FacingRight)
        {
            // ... flip the ennemy.
            Flip();
        }
        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Wander"))
        {
            GetComponentInChildren<Animator>().SetTrigger("StartFollowing");
        }
    }

    public void GravityBeforeJump()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void GravityAfterJump()
    {
        GetComponent<Rigidbody2D>().gravityScale = GravityAfterJumpValue;
    }

    protected void Flip()
    {
        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Follow"))
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.GetChild(0).transform.localScale;
            theScale.x *= -1;
            transform.GetChild(0).transform.localScale = theScale;
        }
    }
}
