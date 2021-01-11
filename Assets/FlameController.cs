using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{

    [SerializeField] private GameObject leftHandFire;
    [SerializeField] private GameObject rightHandFire;

    public void EnableLeftFlame()
    {
        if (GetComponent<Player>().GM.alimMet)
        {
            leftHandFire.GetComponent<ParticleSystem>().Play();
        }
    }
    public void EnableRightFlame()
    {
        if (GetComponent<Player>().GM.alimMet)
        {
            rightHandFire.GetComponent<ParticleSystem>().Play();
        }
    }
    public void DisableLeftFlame()
    {
        if (GetComponent<Player>().GM.alimMet)
        {
            leftHandFire.GetComponent<ParticleSystem>().Stop();
        }

    }

    public void DisableRightFlame()
    {
        if (GetComponent<Player>().GM.alimMet)
        {
            rightHandFire.GetComponent<ParticleSystem>().Stop();
        }
    }
}
