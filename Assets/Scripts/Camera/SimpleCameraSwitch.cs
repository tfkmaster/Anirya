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

    private Vector3 target_cam_pos; 

    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if(SourceCam.Follow == null && SourceCam.CompareTag("FollowCamera"))
        {
            SourceCam.Follow = Anirya;
        }
        else if(TargetCam.Follow == null && TargetCam.CompareTag("FollowCamera"))
        {
            TargetCam.Follow = Anirya;
        }

        if (CSTrigger.position.x < Anirya.position.x)
        {
            SwitchCamera(true);
        }
        else if (CSTrigger.position.x >= Anirya.position.x)
        {
            SwitchCamera(false);
        }

        target_cam_pos = TargetCam.transform.position;
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
