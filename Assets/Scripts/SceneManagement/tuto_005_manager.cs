using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_005_manager : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera FixedCam = default;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera WolfCam = default;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera AniryaCam = default;

    [SerializeField]
    private Wolf wolf = default;

    private GameObject Anirya;


    private float elapsed_time = 0f;
    private bool check_in = true;

    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player");
        Anirya.GetComponent<CharacterMovement>().Interacting = true;
        Anirya.GetComponentInChildren<Animator>().SetBool("interacting", true);
    }

    void Update()
    {
        elapsed_time += Time.deltaTime;

        if (check_in)
        {
            if (elapsed_time >= 1.5f)
            {
                WolfCam.Priority = 15;
            }
            if (elapsed_time >= 3.5f)
            {
                WolfCam.Priority = 5;
                AniryaCam.Priority = 15;
            }
            if (elapsed_time >= 5.5f)
            {
                AniryaCam.Priority = 4;
            }
            if(elapsed_time >= 7f)
            {
                Anirya.GetComponent<CharacterMovement>().Interacting = false;
                Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
                wolf.StartFight();
                check_in = false;
            }
        }
    }
}
