using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : NonHittableActor
{
    void Start()
    {
        //ignore collision on layer Ground
        Physics.IgnoreLayerCollision(9, 9);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().OnHit(this.gameObject, damages);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError(collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // destroy anim calls
    }
}
