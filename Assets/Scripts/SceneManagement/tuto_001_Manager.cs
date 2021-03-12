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
    [SerializeField]
    private GameObject trunk = default;

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

        if (PersistentDatas.tuto_001.TrunkIsBroken)
        {
            Destroy(trunk);
        }
    }

    void Update()
    {
        if (!PersistentDatas.tuto_001.TrunkIsBroken && !trunk)
        {
            PersistentDatas.tuto_001.TrunkIsBroken = true;
        }
    }
}
