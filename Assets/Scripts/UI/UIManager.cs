using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PauseCanvas;
    private GameObject PauseCanvasInstance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PauseCanvasInstance = Instantiate(PauseCanvas, this.transform.position, new Quaternion(0, 0, 0, 0));
        PauseCanvasInstance.SetActive(false);
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
}
