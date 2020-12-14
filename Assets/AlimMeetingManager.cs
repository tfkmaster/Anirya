using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AlimMeetingManager : MonoBehaviour
{
    public GameObject Alim = default;
    public CinemachineVirtualCamera AlimFocusCam = default;
    public UIManager UIManager = default;

    public Dialogue Dialog = default;

    public bool dialogDone = false;

    private bool isPossesed = false;
    private bool launchAlimMeeting = false;
    private bool checkOnFocusCam = false;

    void Start()
    {
        UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    void Update()
    {
        if (launchAlimMeeting)
        {
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
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            launchAlimMeeting = true;
        }
    }
}
