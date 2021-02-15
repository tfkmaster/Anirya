using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyTarget : MonoBehaviour
{
    [SerializeField]
    private Butterfly butterfly = default;
    private bool has_been_reached = false;

    private float timer;

    public Cinemachine.CinemachineVirtualCamera cam = default;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 2f)
        {
            butterfly.SetSpeed(12f);
        }
        if(timer >= 5f)
        {
            cam.Priority = 0;
        }
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !has_been_reached)
        {
            Debug.Log("Target reached");
            has_been_reached = true;
            butterfly.MoveToNextTarget();
        }
    }*/
}
