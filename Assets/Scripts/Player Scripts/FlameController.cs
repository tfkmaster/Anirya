using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{

    [SerializeField] private GameObject leftHandFire;
    [SerializeField] private GameObject rightHandFire;
    [SerializeField] private GameObject leftTrail;
    [SerializeField] private GameObject rightTrail;



    public void EnableLeftFlame()
    {
        
        if (GetComponentInParent<Player>().GM.alimMet)
        {
        leftHandFire.GetComponent<ParticleSystem>().Play();
        leftTrail.SetActive(true);
        }
    }
    public void EnableRightFlame()
    {
        if (GetComponentInParent<Player>().GM.alimMet)
        {
        rightHandFire.GetComponent<ParticleSystem>().Play();
        rightTrail.SetActive(true);
        }
    }
    public void DisableLeftFlame()
    {
        if (GetComponentInParent<Player>().GM.alimMet)
        {
            leftHandFire.GetComponent<ParticleSystem>().Stop();
            leftTrail.GetComponent<TrailRenderer>().Clear();
            leftTrail.SetActive(false);
        }

    }

    public void DisableRightFlame()
    {
        if (GetComponentInParent<Player>().GM.alimMet)
        {
            rightHandFire.GetComponent<ParticleSystem>().Stop();
            rightTrail.GetComponent<TrailRenderer>().Clear();
            rightTrail.SetActive(false);
        }
    }
}
