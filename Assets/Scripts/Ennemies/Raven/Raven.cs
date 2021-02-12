using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
//[RequireComponent(typeof(Rigidbody2D))]
public class Raven : MonoBehaviour
{
    public List<Transform> WanderPoints;
    public float speed = 3.0f;
    public Transform MoveTo;
    private int wanderIndex = 0;
    public float thrust = 10.0f;


    void Start()
    {
        MoveTo = WanderPoints[wanderIndex];       
    }

    void Update()
    {
        //Calculate direction
        Vector3 dir = MoveTo.position - transform.position;

        //Wandering by moving towards
        float step = speed * Time.deltaTime;
        GetComponent<Rigidbody2D>().AddForce(dir * thrust, ForceMode2D.Force);

        if (MoveTo.position == transform.position)
        {
            int newWanderIndex;
            do
            {
                newWanderIndex = Random.Range(0, 9);
            }
            while (newWanderIndex == wanderIndex);

            wanderIndex = newWanderIndex;
            MoveTo = WanderPoints[wanderIndex];
        }

        //Raycasting allowing obstacle avoidance 
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 dir = MoveTo.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(- dir * thrust, ForceMode2D.Impulse);
    }
}
