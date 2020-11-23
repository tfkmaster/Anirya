using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D platform;
    public float timeToGoDown;

    void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow)) timeToGoDown = 0.5f;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if(timeToGoDown <= 0f)
            {
                platform.rotationalOffset = 180f;
                timeToGoDown = 0.5f;
            }
            else
            {
                timeToGoDown -= Time.deltaTime;
            }
        }
    }

    public void setBackEffector()
    {
        platform.rotationalOffset = 0f;
    }
}
