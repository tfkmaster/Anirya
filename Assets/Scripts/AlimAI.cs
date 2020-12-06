using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlimAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 12f;

    public AIDestinationSetter DestinationSetter = default;
    public AIPath Path = default;

    CharacterController2D AniryaController = default;

    void Start()
    {
        AniryaController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        DestinationSetter.target = GameObject.FindGameObjectWithTag("Alim Destination").transform;
        Path.maxSpeed = speed;
    }

    void Update()
    {
        if(Path.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(Path.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        //FlipWhenTargetReached();
    }

    void FlipWhenTargetReached()
    {
        if (Path.reachedDestination)
        {
            if (AniryaController.facingDirection() == 1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (AniryaController.facingDirection() == -1)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }
}
