using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    public Button resume, controls, exit = default;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
