using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gurzil : MonoBehaviour
{
    public GurzilManager GurzilManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GurzilManager.IsPrayInterfaceActive)
        {
            GurzilManager.InteractGuide.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GurzilManager.IsPrayInterfaceActive)
        {
            GurzilManager.InteractGuide.SetActive(true);

            float y_axis = Input.GetAxis("5th Axis");

            if ((y_axis > 0.99) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                GurzilManager.ShowPrayInterface();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GurzilManager.IsPrayInterfaceActive)
        {
            GurzilManager.InteractGuide.SetActive(false);
        }
    }
}
