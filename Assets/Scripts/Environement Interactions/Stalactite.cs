using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : NonHittableActor
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().simulated = true;
        Debug.Log("ddd");

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ddd");

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().OnHit(this.gameObject, damages);
            Debug.Log("Anirya hit !");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // destroy anim calls
    }
}
