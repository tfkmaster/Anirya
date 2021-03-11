using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GurzilManager : MonoBehaviour
{
    public Text Pray;
    public Text Pray2;
    public Text Leave;
    public GameObject InteractGuide;
    public GameObject PrayInterface;
    public GameObject GurzilBlessingInterface;

    public GameObject PrayerChoice;
    public GameObject PrayerAcquired;

    public bool IsPrayInterfaceActive = false;
    public bool IsGurzilBlessingInterface = false;
    private bool reset_x_axis = true;
    public bool isPrayActive = true;
    private GameObject Anirya;

    // Start is called before the first frame update
    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player");
        Leave.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPrayInterfaceActive)
        {
            if (!Anirya.GetComponent<Player>().hasDash)
            {
                PrayerChoice.SetActive(true);
                PrayerAcquired.SetActive(false);
            }
            else
            {
                PrayerChoice.SetActive(false);
                PrayerAcquired.SetActive(true);
            }
            
            float x_axis = Input.GetAxis("Horizontal");
            
            if ((x_axis < -0.2 && reset_x_axis) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                pickButton();
                reset_x_axis = false;
            }
            else if ((x_axis > 0.2 && reset_x_axis) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                pickButton();
                reset_x_axis = false;
            }
            else if (x_axis >= -0.2 && x_axis <= 0.2)
            {
                reset_x_axis = true;
            }

            if (Input.GetKeyDown("joystick button 0"))
            {
                IsPrayInterfaceActive = false;
                PrayInterface.SetActive(false);

                PrayInterface.GetComponent<Animator>().SetBool("Fade In", false);
                PrayInterface.GetComponent<Animator>().SetBool("Fade Out", true);

                if (isPrayActive && !Anirya.GetComponent<Player>().hasDash)
                {
                    Anirya.GetComponent<Player>().hasDash = true;

                    IsGurzilBlessingInterface = true;
                    GurzilBlessingInterface.SetActive(true);
                    GurzilBlessingInterface.GetComponent<Animator>().SetBool("Fade In", true);

                    Anirya.GetComponent<Player>().itijEssencesCarried -= 5;
                }
                else
                {
                    Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
                    Anirya.GetComponent<CharacterMovement>().Interacting = false;
                }
            }
        }
        else if (IsGurzilBlessingInterface && Input.GetKeyDown("joystick button 0"))
        {
            Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
            Anirya.GetComponent<CharacterMovement>().Interacting = false;
            GurzilBlessingInterface.GetComponent<Animator>().SetBool("Fade In", false);
            GurzilBlessingInterface.GetComponent<Animator>().SetBool("Fade Out", true);
            IsGurzilBlessingInterface = false;
        }
       
    }

    void pickButton()
    {
        if (isPrayActive)
        {
            Pray.color = Color.grey;
            Pray2.color = Color.grey;
            Leave.color = Color.white;

            isPrayActive = false;
        }
        else
        {
            Pray.color = Color.white;
            Pray2.color = Color.white;
            Leave.color = Color.grey;

            isPrayActive = true;
        }
    }

    public void ShowPrayInterface()
    {
        InteractGuide.SetActive(false);
        PrayInterface.SetActive(true);
        IsPrayInterfaceActive = true;

        PrayInterface.GetComponent<Animator>().SetBool("Fade In", true);
        PrayInterface.GetComponent<Animator>().SetBool("Fade Out", false);

        Anirya.GetComponent<CharacterMovement>().Interacting = true;
        Anirya.GetComponentInChildren<Animator>().SetBool("interacting", true);
    }

    public void HideInterface()
    {
        GurzilBlessingInterface.SetActive(false);
        PrayInterface.SetActive(false);
    }
}
