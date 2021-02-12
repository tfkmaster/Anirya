using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{

    [SerializeField] private GameObject leftHandFire;
    [SerializeField] private GameObject rightHandFire;

    public void EnableLeftFlame()
    {
        
        /*if (GetComponentInParent<Player>().GM.alimMet)
        {*/
            leftHandFire.GetComponent<ParticleSystem>().Play();
        //}
    }
    public void EnableRightFlame()
    {
        /*if (GetComponentInParent<Player>().GM.alimMet)
        {*/
            rightHandFire.GetComponent<ParticleSystem>().Play();
        //}
    }
    public void DisableLeftFlame()
    {
        /*if (GetComponentInParent<Player>().GM.alimMet)
        {*/
            leftHandFire.GetComponent<ParticleSystem>().Stop();
        //}

    }

    public void DisableRightFlame()
    {
        /*if (GetComponentInParent<Player>().GM.alimMet)
        {*/
            rightHandFire.GetComponent<ParticleSystem>().Stop();
        //}
    }
}
