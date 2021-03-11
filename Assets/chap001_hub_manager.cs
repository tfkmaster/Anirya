using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chap001_hub_manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    public GameObject BlockWaghzenPath = default;
    public GameObject WaghzenPath = default;

    void Awake()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
    }

    void Start()
    {
        if (PersistentDatas.chap001_004.Visited)
        {
            BlockWaghzenPath.SetActive(false);
            WaghzenPath.SetActive(true);
        }
        else
        {
            BlockWaghzenPath.SetActive(true);
            WaghzenPath.SetActive(false);
        }
    }
}
