using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlaceTitle : MonoBehaviour
{
    [SerializeField]
    private string placeTitleLatin = default;
    [SerializeField]
    private string placeTitleTiffinagh = default;
    private float display_time = 5f;
    private bool has_fade = false;
    private bool can_fade = true;

    void Start()
    {
        GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().DisplayPlaceTitle(placeTitleLatin, placeTitleTiffinagh);
    }

    void Update()
    {
        if (!has_fade && can_fade)
        {
            display_time -= Time.deltaTime;
            if (display_time <= 0f)
            {
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().FadePlaceTitle(true);
                has_fade = true;
            }
        }

        if (has_fade && can_fade)
        {
            display_time += Time.deltaTime;
            if(display_time >= 0f)
            {
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().FadePlaceTitle(false);
                can_fade = false;
            }
        }
    }
}
