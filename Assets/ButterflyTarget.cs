using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyTarget : MonoBehaviour
{
    private Butterfly butterfly = default;
    private bool has_been_reached = false;

    void Start()
    {
        butterfly = GameObject.FindGameObjectWithTag("Butterfly").GetComponent<Butterfly>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !has_been_reached)
        {
            Debug.Log("Target reached");
            has_been_reached = true;
            butterfly.MoveToNextTarget();
        }
    }
}
