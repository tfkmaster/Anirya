using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlimMeetingManager : MonoBehaviour
{
    public GameObject Alim = default;
    public CinemachineVirtualCamera AlimFocusCam = default;
    public UIManager UIManager = default;

    public CanvasGroup DidacticCanvas = default;

    public Dialogue Dialog = default;

    public ParticleSystem TallamShadow;

    public bool dialogDone = false;

    private bool isPossesed = false;
    private bool launchAlimMeeting = false;
    private bool checkOnFocusCam = false;

    private bool display_UI = false;
    private bool UI_on = false;
    private bool remove_UI = false;

    void Start()
    {
        UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    void Update()
    {
        if (display_UI)
        {
            UI_on = true;
            StopAllCoroutines();
            StartCoroutine(FadeCanvas(DidacticCanvas, 0f, 1f, 1.2f, true));
            display_UI = false;
        }
        if (UI_on && (Input.GetKeyDown("joystick button 0")))
        {
            UI_on = false;
            remove_UI = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().Interacting = false;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetBool("interacting", false);
        }
        if (remove_UI)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCanvas(DidacticCanvas, 1f, 0f, 1.2f, false));
            remove_UI = false;
        }

        if (launchAlimMeeting && !GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().alimMet)
        {
            TallamShadow.Play();
            TallamShadow.GetComponentInChildren<Collider2D>().isTrigger = false;
            AlimFocusCam.Priority = 12;
            Alim.GetComponentInChildren<SpriteRenderer>().enabled = true;
            Alim.GetComponentInChildren<Animator>().SetTrigger("leaveJail");
        }

        if (launchAlimMeeting && UIManager.GetDialogueManager().dialogEnded == true)
        {
            UIManager.SetActiveDialog(false);
            AlimFocusCam.Priority = 8;
            Alim.GetComponentInChildren<Animator>().SetTrigger("toNewIdle");
            Alim.GetComponent<AlimAI>().DestinationSetter.target = GameObject.FindGameObjectWithTag("Alim Destination").transform;
            launchAlimMeeting = false;

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().alimMet = true;

            UIManager.PlayerStatsCanvasInstance.SetActive(true);
            UIManager.PlayerStatsCanvasInstance.GetComponent<Animator>().SetBool("Pop Player Stats", true);
            
        }
    }

    public void DisplayUI()
    {
        display_UI = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().alimMet)
        {
            launchAlimMeeting = true;
        }
    }

    public static IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration, bool activate)
    {
        Debug.Log("FADING");
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        canvas.alpha = startAlpha;

        if (activate)
        {
            canvas.gameObject.SetActive(true);
        }

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if (startAlpha > endAlpha)
            {
                canvas.alpha = startAlpha - percentage;
            }
            else
            {
                canvas.alpha = startAlpha + percentage;
            }
            yield return new WaitForEndOfFrame();
        }

        canvas.alpha = endAlpha;
        if (!activate)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
