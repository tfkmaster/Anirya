using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseCanvas;
    private GameObject PauseCanvasInstance;

    public GameObject ControlsCanvas;
    private GameObject ControlsCanvasInstance;

    public GameObject PlayerStatsCanvas;
    private GameObject PlayerStatsCanvasInstance;

    public GameObject DialogCanvas;
    private GameObject DialogCanvasInstance;

    public bool controlScreenOn = false;

    [SerializeField]
    public GameManager GM;

    private bool reset_y_axis = true;

    public Color[] ButtonColors = new Color[]
    {
        new Color32(201, 233, 235, 255), //blue
        new Color32(255, 255, 255, 255), //white
        new Color32(0, 0, 0, 255)        //black
    };

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        PauseCanvasInstance = Instantiate(PauseCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        PauseCanvasInstance.GetComponent<PauseCanvas>().UIManager = gameObject.GetComponent<UIManager>();
        PauseCanvasInstance.SetActive(false);

        //Adding Event Listeners to Pause Canvas buttons 
        PauseCanvasInstance.GetComponent<PauseCanvas>().Buttons[0].GetComponent<Button>().onClick.AddListener(resume_button_action);
        PauseCanvasInstance.GetComponent<PauseCanvas>().Buttons[1].GetComponent<Button>().onClick.AddListener(show_controls_button_action);
        PauseCanvasInstance.GetComponent<PauseCanvas>().Buttons[2].GetComponent<Button>().onClick.AddListener(exit_game_button_action);

        //Window showing player controls
        ControlsCanvasInstance = Instantiate(ControlsCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(ControlsCanvasInstance);
        ControlsCanvasInstance.GetComponentInChildren<Button>().onClick.AddListener(back_to_pause_menu_button_action);
        ControlsCanvasInstance.SetActive(false);

        //Canvas where player stats are rendered
        PlayerStatsCanvasInstance = Instantiate(PlayerStatsCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(PlayerStatsCanvasInstance);
        PlayerStatsCanvasInstance.SetActive(true);

        //Canvas where dialogs are displayed
        DialogCanvasInstance = Instantiate(DialogCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(DialogCanvasInstance);
        DialogCanvasInstance.SetActive(false);
    }

    void Update()
    {
        if(controlScreenOn && (Input.GetKeyDown("joystick button 2")))
        {
            back_to_pause_menu_button_action();
        }
        if (controlScreenOn && (Input.GetKeyDown("joystick button 0")))
        {
            ControlsCanvasInstance.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.Invoke();
        }

        //UI navigation with directional pad
        float y_axis = Input.GetAxis("Vertical");

        if(y_axis < -0.2 && reset_y_axis)
        {
            PauseCanvasInstance.GetComponent<PauseCanvas>().SelectNextButton();
            reset_y_axis = false;
        }
        else if(y_axis > 0.2 && reset_y_axis)
        {
            PauseCanvasInstance.GetComponent<PauseCanvas>().SelectPreviousButton();
            reset_y_axis = false;
        }
        else if(y_axis >= -0.2 && y_axis <= 0.2)
        {
            reset_y_axis = true;
        }
    }


    public void DisplayPauseMenu(bool _display)
    {
        if (_display)
        {
            PauseCanvasInstance.SetActive(true);
            PauseCanvasInstance.GetComponent<PauseCanvas>().ResetActiveButton();
            PlayerStatsCanvasInstance.SetActive(false);
        }
        else
        {
            PauseCanvasInstance.SetActive(false);
            PlayerStatsCanvasInstance.SetActive(true);
        }
    }

    public void resume_button_action()
    {
        GM.SetPause();
    }

    public void exit_game_button_action()
    {
        GM.ExitGame();
    }

    public void show_controls_button_action()
    {
        controlScreenOn = true;
        ButtonHover(ControlsCanvasInstance.transform.GetChild(3).gameObject, true);
        PauseCanvasInstance.SetActive(false);
        ControlsCanvasInstance.SetActive(true);
    }

    public void back_to_pause_menu_button_action()
    {
        controlScreenOn = false;
        ControlsCanvasInstance.SetActive(false);
        PauseCanvasInstance.SetActive(true);
    }

    public void SendPlayerStatsToPlayerStatsManager(int maxHealth, int actualHealth, float maxHeat, float actualHeat)
    {
        PlayerStatsCanvasInstance.GetComponent<PlayerStatsManager>().updateVisual(maxHealth, actualHealth, maxHeat, actualHeat);
    }

    public void ButtonHover(GameObject button, bool on_hover)
    {
        if (on_hover)
        {
            button.GetComponentInChildren<Text>().color = ButtonColors[1];
            button.GetComponent<Image>().color = ButtonColors[0];
        }
        else
        {
            button.GetComponentInChildren<Text>().color = ButtonColors[2];
            button.GetComponent<Image>().color = ButtonColors[1];
        }
        return;
    }

    public void SetActiveDialog(bool set)
    {
        DialogCanvasInstance.SetActive(set);
    }

    public DialogueManager GetDialogueManager()
    {
        return DialogCanvasInstance.GetComponentInChildren<DialogueManager>();
    }
}   
