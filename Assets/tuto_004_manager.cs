using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_004_manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    [SerializeField]
    private GameObject Ennemies = default;

    void Start()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();

        if (PersistentDatas.tuto_005.FirstWolfSlayed)
        {
            Ennemies.SetActive(true);
        }
    }
}
