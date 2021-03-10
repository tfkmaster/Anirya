using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderManager : MonoBehaviour
{
    private List<Transform> WanderPoints;

    void Start()
    {
        WanderPoints = new List<Transform>();
        int WanderPointCount = transform.childCount;
        //Debug.Log("Count : " + WanderPointCount);

        for (int i = 0; i < WanderPointCount; ++i)
        {
            //Debug.Log("bite : " + transform.GetChild(i).name);
            WanderPoints.Add(transform.GetChild(i));
        }
    }

    public List<Transform> GetWanderPoints()
    {
        return WanderPoints;
    }
}
