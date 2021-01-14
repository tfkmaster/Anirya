using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    public float speed = 0f;
    [SerializeField]
    private Transform[] destinationTargets = default;

    private int target_index = 0;

    public AIDestinationSetter DestinationSetter = default;
    public AIPath Path = default;
    public GameObject Anirya = default;

    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player");
        DestinationSetter.target = destinationTargets[0];
        Path.maxSpeed = speed;
    }

    void Update()
    {
        if(Path.reachedDestination)
        {
            GetComponentInChildren<Animator>().SetBool("isFlying", false);
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("isFlying", true);
        }
    }

    public void SetSpeed(float spd)
    {
        Path.maxSpeed = spd;
    }

    public void MoveToNextTarget()
    {
        ++target_index;
        DestinationSetter.target = destinationTargets[target_index];
    }
}
