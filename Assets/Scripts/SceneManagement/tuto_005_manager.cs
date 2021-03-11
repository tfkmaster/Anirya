using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_005_manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera FixedCam = default;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera WolfCam = default;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera AniryaCam = default;

    [SerializeField]
    private Wolf wolf = default;

    [SerializeField]
    private GameObject TallamShadows = default;
    [SerializeField]
    private GameObject SceneLoader = default;
    [SerializeField]
    private GameObject InvisbleWall = default;

    private GameObject Anirya;


    private float elapsed_time = 0f;
    private bool check_in = true;

    void Start()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
        Anirya = GameObject.FindGameObjectWithTag("Player");

        if(!PersistentDatas.tuto_005.FirstWolfSlayed)
        {
            Anirya.GetComponent<CharacterMovement>().Interacting = true;
            Anirya.GetComponentInChildren<Animator>().SetBool("interacting", true);
            InvisbleWall.SetActive(true);
        }
        else
        {
            InvisbleWall.SetActive(false);
            SceneLoader.SetActive(true);
            Destroy(wolf.gameObject);
            Destroy(TallamShadows);
        }
    }

    void Update()
    {
        elapsed_time += Time.deltaTime;

        if (check_in && !PersistentDatas.tuto_005.FirstWolfEncountered)
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

                if (!PersistentDatas.tuto_005.FirstWolfEncountered)
                {
                    PersistentDatas.tuto_005.FirstWolfEncountered = true;
                }

                wolf.StartFight();
                check_in = false;
            }
        }
        else if(!PersistentDatas.tuto_005.FirstWolfSlayed && elapsed_time >= 1.0f)
        {
            Anirya.GetComponent<CharacterMovement>().Interacting = false;
            Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
            wolf.StartFight();
        }

        if (wolf.GetComponent<Wolf>().GetDead())
        {
            PersistentDatas.tuto_005.FirstWolfSlayed = true;
            SceneLoader.SetActive(true);
            InvisbleWall.SetActive(false);
            Destroy(TallamShadows);
        }
    }
}
