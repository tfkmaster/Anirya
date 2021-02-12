using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Raven : MonoBehaviour
{
    public List<Transform> WanderPoints;
    public float speed = 3.0f;
    public Transform MoveTo;
    private int wanderIndex = 0;

    void Start()
    {
        MoveTo = WanderPoints[wanderIndex];       
    }

    void Update()
    {
        //Wandering by moving towards
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, MoveTo.position, step);

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
}
