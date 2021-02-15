using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject butterfly = default;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            butterfly.GetComponent<Butterfly>().SecondSpawn();
            gameObject.SetActive(false);
        }
    }
}
