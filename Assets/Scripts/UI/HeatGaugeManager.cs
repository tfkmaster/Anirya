using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatGaugeManager : MonoBehaviour
{
    public RectTransform waver;
    public Image filler;
    public Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = waver.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (filler.fillAmount >= 0.01f)
        {
            waver.anchoredPosition = new Vector2(startPos.x, startPos.y+10 + filler.fillAmount * 100);
        }
        else
        {
            waver.anchoredPosition = new Vector2(startPos.x, startPos.y);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            filler.fillAmount += 0.15f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            filler.fillAmount -= 0.15f;
        }

    }
}
