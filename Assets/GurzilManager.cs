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

    public bool IsPrayInterfaceActive = false;
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
                if (isPrayActive)
                {

                }
                else
                {
                    PrayInterface.SetActive(false);
                    Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
                    Anirya.GetComponent<CharacterMovement>().Interacting = false;
                }
            }
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


        Anirya.GetComponent<CharacterMovement>().Interacting = true;
        Anirya.GetComponentInChildren<Animator>().SetBool("interacting", true);
    }
}
