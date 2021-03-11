using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chap001_hub_manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    public GameObject BlockWaghzenPath = default;
    public GameObject WaghzenPath = default;

    private bool temp = true;

    void Awake()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
    }

    void Start()
    {
        if (!temp) //meaning last hallway has been crossed
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
