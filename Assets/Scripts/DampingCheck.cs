using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampingCheck : MonoBehaviour
{
    public List<Cinemachine.CinemachineVirtualCamera> VCAMS = default;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetDamping(VCAMS, 2f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetDamping(VCAMS, 0f);
        }
    }

    void SetDamping(List<Cinemachine.CinemachineVirtualCamera> cameras, float damping)
    {
        foreach(Cinemachine.CinemachineVirtualCamera cam in cameras)
        {
            cam.GetComponent<Cinemachine.CinemachineConfiner>().m_Damping = damping;
        }
    }
}
