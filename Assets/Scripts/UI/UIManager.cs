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
    public GameObject PlayerStatsCanvasInstance;

    public GameObject DialogCanvas;
    private GameObject DialogCanvasInstance;

    public GameObject PlaceTitleCanvas;
    private GameObject PlaceTitleCanvasInstance;

    public bool controlScreenOn = false;

    [SerializeField]
    public GameManager GM;

    private bool reset_y_axis = true;

    public Color[] ButtonColors = new Color[]
    {
        new Color32(168, 78, 123, 255), //pink anirya
        new Color32(201, 233, 235, 255), //blue
        new Color32(255, 255, 255, 255), //white
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
        ControlsCanvasInstance.SetActive(false);

        //Canvas where player stats are rendered
        PlayerStatsCanvasInstance = Instantiate(PlayerStatsCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(PlayerStatsCanvasInstance);
        PlayerStatsCanvasInstance.GetComponent<CanvasGroup>().alpha = 1f;

        //Canvas where dialogs are displayed
        DialogCanvasInstance = Instantiate(DialogCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(DialogCanvasInstance);
        DialogCanvasInstance.SetActive(false);

        //Canvas where place titles are displayed
        PlaceTitleCanvasInstance = Instantiate(PlaceTitleCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(PlaceTitleCanvasInstance);
        PlaceTitleCanvasInstance.SetActive(false);
    }

    void Update()
    {
        if(controlScreenOn && ((Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Escape))))
        {
            Debug.Log("Escaping");
            ControlsCanvasInstance.GetComponent<ControlsCanvas>().ResetActiveButton();
            back_to_pause_menu_button_action();
        }

        if (!controlScreenOn)
        {
            //UI navigation with directional pad
            float y_axis = Input.GetAxis("Vertical");

            if ((y_axis < -0.2 && reset_y_axis) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                PauseCanvasInstance.GetComponent<PauseCanvas>().SelectNextButton();
                reset_y_axis = false;
            }
            else if ((y_axis > 0.2 && reset_y_axis) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                PauseCanvasInstance.GetComponent<PauseCanvas>().SelectPreviousButton();
                reset_y_axis = false;
            }
            else if (y_axis >= -0.2 && y_axis <= 0.2)
            {
                reset_y_axis = true;
            }
        }
    }


    public void DisplayPauseMenu(bool _display)
    {
        if (_display)
        {
            PauseCanvasInstance.SetActive(true);
            PauseCanvasInstance.GetComponent<PauseCanvas>().ResetActiveButton();
            PlayerStatsCanvasInstance.GetComponent<CanvasGroup>().alpha = 0f;
            PlaceTitleCanvasInstance.SetActive(false);

            DialogCanvasInstance.SetActive(false);
        }
        else
        {
            PauseCanvasInstance.SetActive(false);

            PlayerStatsCanvasInstance.GetComponent<CanvasGroup>().alpha = 1f;



            if (DialogCanvasInstance.GetComponent<DialogueManager>().InProgress())

            {

                DialogCanvasInstance.GetComponent<DialogueManager>().ForceTypeSentence();

                DialogCanvasInstance.SetActive(true);

            }

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

    public void SetActiveDialog(bool set)
    {
        DialogCanvasInstance.SetActive(set);
    }

    public DialogueManager GetDialogueManager()
    {
        return DialogCanvasInstance.GetComponentInChildren<DialogueManager>();
    }

    public void DisplayPlaceTitle(string latin, string tiffinagh)
    {
        PlaceTitleCanvasInstance.transform.GetChild(1).GetComponent<Text>().text = latin;
        PlaceTitleCanvasInstance.transform.GetChild(2).GetComponent<Text>().text = tiffinagh;
        PlaceTitleCanvasInstance.SetActive(true);
    }

    public void FadePlaceTitle(bool fadein)
    {
        PlaceTitleCanvasInstance.GetComponent<Animator>().SetBool("end", fadein);
        PlaceTitleCanvasInstance.GetComponent<Animator>().SetBool("start", !fadein);
    }

    public void FadeTutoGuide(GameObject TutoGuide, bool fadein)
    {
        TutoGuide.GetComponent<Animator>().SetBool("end", fadein);
        TutoGuide.GetComponent<Animator>().SetBool("start", !fadein);
    }
}   
