using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject[] Buttons = default;
    private GameObject[] go = default;
    private int active_button_index = 0;
    private bool reset_y_axis = true;
    private bool credits_on = false;

    [SerializeField]
    private GameObject Credits = default;

    void Awake()
    {
        Buttons[0].GetComponent<Button>().onClick.AddListener(LaunchGame);
        Buttons[1].GetComponent<Button>().onClick.AddListener(DisplayCredits);
        Buttons[2].GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    void Start()
    {
        ResetActiveButton();
    }

    void Update()
    {
        //custom button onClick activated
        if (Input.GetKeyDown("joystick button 0"))
        {
            Buttons[active_button_index % 3].GetComponent<Button>().onClick.Invoke();
        }

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

        if (credits_on && Input.GetKeyDown("joystick button 1"))
        {
            foreach (GameObject g in go)
            {
                g.SetActive(true);
            }
            Credits.SetActive(false);
            credits_on = false;
        }
    }

    public void ButtonHover(GameObject button, bool on_hover)
    {
        if (on_hover)
        {
            button.GetComponentInChildren<Text>().color = new Color32(168, 78, 123, 255);
            button.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            button.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
            button.transform.GetChild(0).gameObject.SetActive(false);
        }
        return;
    }

    public int GetActiveButtonIndex()
    {
        return active_button_index;
    }

    public void SelectNextButton()
    {
        ButtonHover(Buttons[active_button_index % 3], false);
        ++active_button_index;
        ButtonHover(Buttons[active_button_index % 3], true);
    }

    public void SelectPreviousButton()
    {
        ButtonHover(Buttons[active_button_index % 3], false);
        if (active_button_index == 0) active_button_index = 3;
        --active_button_index;
        ButtonHover(Buttons[active_button_index % 3], true);
    }

    public void ResetActiveButton()
    {
        active_button_index = 0;
        ButtonHover(Buttons[active_button_index], true);
        ButtonHover(Buttons[1], false);
        ButtonHover(Buttons[2], false);
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene("1");
    }

    public void DisplayCredits()
    {
        go = GameObject.FindGameObjectsWithTag("Start Menu UI");
        foreach (GameObject g in go)
        {
            g.SetActive(false);
        }
        Credits.SetActive(true);
        credits_on = true;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
