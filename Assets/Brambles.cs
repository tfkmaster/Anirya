using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brambles : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("DeadActor")){
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<Player>().OnHit(gameObject, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.transform.name);
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("DeadActor"))
        {
            //Debug.Log("Player hit");
            collision.gameObject.GetComponent<Player>().OnHit(gameObject, 1);
        }
    }
}
