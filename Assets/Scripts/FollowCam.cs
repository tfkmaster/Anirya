using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        if (cam.CompareTag("FollowCamera") && cam.Follow == null)
        {
            cam.Follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }
}
