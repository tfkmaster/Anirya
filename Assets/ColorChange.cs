using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform startPoint;
    public Transform endPoint;
    public ColorChanger[] colorChangers;
    public SpriteRenderer[] renderers;
    const float MAX_DISTANCE = 20000;
    public Light2D globalLight;
    private float lightIntensity;

    public float baseLight = 1f;
    public float endLight = 0.7f;

    

    private void Start()
    {
        for(int i = 0; i< renderers.Length; i++)
        {
            colorChangers[i].spr = renderers[i];
        }
    }

    void Update()
    {
        if(renderers.Length != 0)
        {
            //Get distance between those two Objects
            float distanceApart = getSqrDistance(GetComponentInParent<Transform>().position, endPoint.position);

            //Convert 0 and 200 distance range to 0f and 1f range
            float lerp = mapValue(distanceApart, 0, MAX_DISTANCE, 0f, 1f);
            lightIntensity = Mathf.Lerp(endLight, baseLight, lerp);
            globalLight.intensity = lightIntensity;
            foreach (ColorChanger colChan in colorChangers)
            {
                Color lerpColor = Color.Lerp(colChan.ColorToReach, colChan.baseColor, lerp);
                colChan.spr.color = lerpColor;
            }
        }    
    }

    public float getSqrDistance(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).sqrMagnitude;
    }

    float mapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }
}


