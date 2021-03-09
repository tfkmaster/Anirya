using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    public int maxHealth;
    public int actualHealth;

    public float maxHeat;
    public float actualHeat;

    public GameObject HeatGauge;
    public GameObject Lives;

    public Sprite emptyHeart;
    public Sprite fullHeart;

    public Text itijText;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            itijText.text = player.itijEssencesCarried.ToString();
        }
    }

    public void updateVisual(int maxHealthSent, int actualHealthSent, float maxHeatSent, float actualHeatSent)
    {
        //Max Health modification
        if(maxHealthSent != maxHealth)
        {
            maxHealth = maxHealthSent;
            for(int i = 0; i < maxHealth; i++)
            {
                Lives.transform.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = maxHealth; i < Lives.transform.childCount; i++)
            {
                Lives.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //Actual Health modifications
        if (actualHealthSent != actualHealth)
        {
            actualHealth = actualHealthSent;
            for (int i = 0; i < actualHealth; i++)
            {
                Lives.transform.GetChild(i).GetComponent<Image>().sprite = fullHeart;
            }
            for (int i = actualHealth; i < maxHealth; i++)
            {
                Lives.transform.GetChild(i).GetComponent<Image>().sprite = emptyHeart;
            }   
        }
        //Max Heat modification
        if (maxHeatSent != maxHeat)
        {
            maxHeat = maxHeatSent;
        }

        //Actual Heat modification
        if (actualHeatSent != actualHeat)
        {
            actualHeat = actualHeatSent;
            HeatGauge.GetComponent<HeatGaugeManager>().filler.fillAmount = actualHeat / maxHeat;
        }
    }

    public void DisplayUI()
    {
        GameObject alim_meeting_manager = GameObject.FindGameObjectWithTag("Alim Meeting Manager");
        alim_meeting_manager.GetComponent<AlimMeetingManager>().DisplayUI();
    }
}
