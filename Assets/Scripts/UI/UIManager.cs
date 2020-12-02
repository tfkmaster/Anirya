using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseCanvas;
    private GameObject PauseCanvasInstance;
    [SerializeField]
    public GameManager GM;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        PauseCanvasInstance = Instantiate(PauseCanvas, this.transform.position, new Quaternion(0, 0, 0, 0));
        PauseCanvasInstance.SetActive(false);
        PauseCanvasInstance.GetComponent<PauseCanvas>().resume.onClick.AddListener(resume_button_action);
    }


    public void DisplayPauseMenu(bool _display)
    {
        if (_display)
        {
            PauseCanvasInstance.SetActive(true);
        }
        else
        {
            PauseCanvasInstance.SetActive(false);
        }
    }

    public void resume_button_action()
    {
        Debug.Log("TAMERE");
        GM.SetPause();
    }
}
