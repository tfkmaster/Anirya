using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InitCameras : MonoBehaviour
{
    public CinemachineVirtualCamera FollowRight;
    public CinemachineVirtualCamera FollowLeft;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        FollowRight.Follow = player.transform;
        FollowLeft.Follow = player.transform;
    }
}
