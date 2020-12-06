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

    [SerializeField]
    public GameManager GM;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        PauseCanvasInstance = Instantiate(PauseCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        PauseCanvasInstance.SetActive(false);

        //Adding Event Listeners to Pause Canvas buttons 
        PauseCanvasInstance.GetComponent<PauseCanvas>().resume.onClick.AddListener(resume_button_action);
        PauseCanvasInstance.GetComponent<PauseCanvas>().exit.onClick.AddListener(exit_game_button_action);
        PauseCanvasInstance.GetComponent<PauseCanvas>().controls.onClick.AddListener(show_controls_button_action);

        //Window showing player controls
        ControlsCanvasInstance = Instantiate(ControlsCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(ControlsCanvasInstance);
        ControlsCanvasInstance.GetComponentInChildren<Button>().onClick.AddListener(back_to_pause_menu_button_action);
        ControlsCanvasInstance.SetActive(false);

        //Canvas where player stats are rendered
        PlayerStatsCanvasInstance = Instantiate(PlayerStatsCanvas, this.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        DontDestroyOnLoad(PlayerStatsCanvasInstance);
        PlayerStatsCanvasInstance.SetActive(true);
    }


    public void DisplayPauseMenu(bool _display)
    {
        if (_display)
        {
            PauseCanvasInstance.SetActive(true);
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
        PauseCanvasInstance.SetActive(false);
        ControlsCanvasInstance.SetActive(true);
    }

    void back_to_pause_menu_button_action()
    {
        ControlsCanvasInstance.SetActive(false);
        PauseCanvasInstance.SetActive(true);
    }
}   
