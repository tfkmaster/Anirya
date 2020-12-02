using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject PauseMenu;

    void Awake()
    {
        PauseMenu.SetActive(false);
    }

    public void DisplayPauseMenu(bool _display)
    {
        if (_display)
        {
            PauseMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(false);
        }
    }
}
