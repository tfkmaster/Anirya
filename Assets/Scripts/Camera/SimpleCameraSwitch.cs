using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SimpleCameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera SourceCam;
    public CinemachineVirtualCamera TargetCam;

    public Transform Anirya = default;

    [SerializeField]
    private Transform CSTrigger = default;

    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if(CSTrigger.position.x < Anirya.position.x && CSTrigger.GetComponent<CameraSwitchTrigger>().isActive)
        {
            SwitchCamera(true);
        }
        else if(CSTrigger.position.x >= Anirya.position.x && CSTrigger.GetComponent<CameraSwitchTrigger>().isActive)
        {
            SwitchCamera(false);
        }
    }

    void SwitchCamera(bool switch_camera)
    {
        if (switch_camera)
        {
            SourceCam.Priority = 9;
            TargetCam.Priority = 10;
        }
        else
        {
            TargetCam.Priority = 9;
            SourceCam.Priority = 10;
        }
    }
}
