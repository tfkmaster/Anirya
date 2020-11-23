using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SimpleCameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera SourceCam;
    public CinemachineVirtualCamera TargetCam;

    private bool has_switch;
    private bool is_allowed_to_switch;

    void Start()
    {
        has_switch = false;
        is_allowed_to_switch = true;
    }

    public void SwitchCamera()
    {
        if (!has_switch)
        {
            TargetCam.Priority = 11;
            SourceCam.Priority = 10;
            has_switch = true;
        }
        else
        {
            SourceCam.Priority = 11;
            TargetCam.Priority = 10;
            has_switch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && is_allowed_to_switch)
        {
            SwitchCamera();
            is_allowed_to_switch = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!is_allowed_to_switch)
        {
            //if(has_switch && collision.gameObject.GetComponent<Transform>().position.x >=)
            is_allowed_to_switch = true;
        }
    }
}
