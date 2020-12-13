﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHelpers : MonoBehaviour
{
    public GameObject GM;
    private GameObject GM_Instance;
    void Start()
    {
        #if UNITY_EDITOR
            if(GameObject.FindGameObjectWithTag("GameManager") == null)
            {
                GM_Instance = Instantiate(GM, this.transform.position, new Quaternion(0, 0, 0, 0));
            }
        #endif
    }
}
