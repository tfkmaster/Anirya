using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : NonHittableActor
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
