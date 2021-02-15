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

    [SerializeField]
    private Transform spawn_point = default;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cam = default;
    private float timer = 0f;

    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>().tuto_001.ButterflyCinematic)
        {
            Anirya.GetComponentInChildren<Animator>().SetBool("interacting", true);
            Anirya.GetComponent<CharacterMovement>().Interacting = true;
        }

        DestinationSetter.target = destinationTargets[0];
        Path.maxSpeed = speed;
    }

    void Update()
    {
        if(transform.position.y >= destinationTargets[0].transform.position.y - 0.5f && timer >= 1.5f)
        {
            transform.position = spawn_point.position;
            DestinationSetter.target = spawn_point;
        }
        if (Path.reachedDestination)
        {
            timer += Time.deltaTime;
            //GetComponentInChildren<Animator>().SetBool("isFlying", false);
            //transform.position = spawn_point.position;
            //DestinationSetter.target = spawn_point;
            //cam.Priority = 0;
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

    public void SecondSpawn()
    {
        spawn_point.position = new Vector3(spawn_point.position.x + 12f, spawn_point.position.y + 7f, spawn_point.position.z);
    }
}
