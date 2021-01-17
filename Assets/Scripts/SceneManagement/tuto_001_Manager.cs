using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_001_Manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    [SerializeField]
    private GameObject ButterflyManagement = default;
    [SerializeField]
    private GameObject TutoGuideManagement = default;

    void Awake()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
    }

    void Start()
    {
        if (PersistentDatas.tuto_001.ButterflyCinematic == false)
        {
            Destroy(ButterflyManagement);
        }
        else
        {
            PersistentDatas.tuto_001.ButterflyCinematic = false;
        }

        if(PersistentDatas.tuto_001.TutorialGuide == false)
        {
            Destroy(TutoGuideManagement);
        }
        else
        {
            PersistentDatas.tuto_001.TutorialGuide = false;
        }
    }
}
