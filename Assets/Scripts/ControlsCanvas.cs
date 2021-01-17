using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsCanvas : MonoBehaviour
{
    public GameObject[] Buttons = default;
    public UIManager UIManager;

    [SerializeField]
    private GameObject[] ControlImages = default;

    private int active_button_index = 0;
    private bool reset_y_axis = true;

    void Start()
    {
        UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        ResetActiveButton();
    }

    void Update()
    {
        if (UIManager.controlScreenOn)
        {
            //UI navigation with directional pad
            float y_axis = Input.GetAxis("Vertical");

            if ((y_axis < -0.2 && reset_y_axis) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectNextButton();
                reset_y_axis = false;
            }
            else if ((y_axis > 0.2 && reset_y_axis) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectPreviousButton();
                reset_y_axis = false;
            }
            else if (y_axis >= -0.2 && y_axis <= 0.2)
            {
                reset_y_axis = true;
            }

            DisplayImage(active_button_index);
        }
    }

    void DisplayImage(int index)
    {
        ControlImages[index % 2].SetActive(true);
        ControlImages[(index + 1) % 2].SetActive(false);
    }

    public int GetActiveButtonIndex()
    {
        return active_button_index;
    }

    public void SelectNextButton()
    {
        UIManager.ButtonHover(Buttons[active_button_index % 2], false);
        ++active_button_index;
        UIManager.ButtonHover(Buttons[active_button_index % 2], true);
    }

    public void SelectPreviousButton()
    {
        UIManager.ButtonHover(Buttons[active_button_index % 2], false);
        if (active_button_index == 0) active_button_index = 2;
        --active_button_index;
        UIManager.ButtonHover(Buttons[active_button_index % 2], true);
    }

    public void ResetActiveButton()
    {
        active_button_index = 0;
        UIManager.ButtonHover(Buttons[active_button_index], true);
        UIManager.ButtonHover(Buttons[1], false);
    }
}
