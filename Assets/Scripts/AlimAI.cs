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

    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("Alim").Length >= 2)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        AniryaController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        //DestinationSetter.target = GameObject.FindGameObjectWithTag("Alim Destination").transform;
        Path.maxSpeed = speed;
    }

    void Update()
    {   
        float x_diff = AniryaController.transform.position.x - this.transform.position.x;

        if(x_diff >= 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if(x_diff < 0f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }
}
