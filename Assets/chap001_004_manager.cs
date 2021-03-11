using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chap001_004_manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;

    void Start()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PersistentDatas.chap001_004.Visited = true;
        }
    }
}
