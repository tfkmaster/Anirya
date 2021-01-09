using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseCanvas : MonoBehaviour
{
    public GameObject[] Buttons = default;
    public UIManager UIManager;
    private int active_button_index = 0;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetActiveButton();
    }

    void Update()
    {
        //custom button onClick activated
        if(Input.GetKeyDown("joystick button 0"))
        {
            Buttons[active_button_index % 3].GetComponent<Button>().onClick.Invoke();
        }
        //allows to Get Back In Game From B button 
        if(Input.GetKeyDown("joystick button 2"))
        {
            Buttons[0].GetComponent<Button>().onClick.Invoke();
        }
    }

    public int GetActiveButtonIndex()
    {
        return active_button_index;
    }



    public void SelectNextButton()
    {
        UIManager.ButtonHover(Buttons[active_button_index % 3], false);
        ++active_button_index;
        UIManager.ButtonHover(Buttons[active_button_index % 3], true);
    }

    public void SelectPreviousButton()
    {
        UIManager.ButtonHover(Buttons[active_button_index % 3], false);
        if (active_button_index == 0) active_button_index = 3;
        --active_button_index;
        UIManager.ButtonHover(Buttons[active_button_index % 3], true);
    }

    public void ResetActiveButton()
    {
        active_button_index = 0;
        UIManager.ButtonHover(Buttons[active_button_index], true);
        UIManager.ButtonHover(Buttons[1], false);
        UIManager.ButtonHover(Buttons[2], false);
    }
}
