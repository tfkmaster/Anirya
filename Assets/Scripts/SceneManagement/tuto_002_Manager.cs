using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_002_Manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    private Player player = default;
    [SerializeField]
    private GameObject trunk = default;
    [SerializeField]
    private GameObject tallam_shadows = default;
    void Awake()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        if (PersistentDatas.tuto_002.TrunkIsBroken)
        {
            Destroy(trunk);
        }

        if (player.GM.alimMet && !PersistentDatas.tuto_003.FirstWolfSlayed)
        {
            tallam_shadows.GetComponent<ParticleSystem>().Play();
            tallam_shadows.GetComponentInChildren<Collider2D>().isTrigger = false;
        }
    }

    void Update()
    {
        if (!PersistentDatas.tuto_002.TrunkIsBroken && !trunk)
        {
            PersistentDatas.tuto_002.TrunkIsBroken = true;
        }
    }
}
