using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gurzil : MonoBehaviour
{
    public GurzilManager GurzilManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GurzilManager.IsPrayInterfaceActive && !GurzilManager.IsGurzilBlessingInterface)
        {
            GurzilManager.InteractGuide.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GurzilManager.IsPrayInterfaceActive && !GurzilManager.IsGurzilBlessingInterface)
        {
            float y_axis = Input.GetAxis("5th Axis");
            float y_axis_variant = Input.GetAxis("Vertical");
            float x_axis = Input.GetAxis("Horizontal");

            if ((y_axis > 0.99f) || Input.GetKeyDown(KeyCode.UpArrow) || (y_axis_variant > 0.99f && (x_axis <= 0.2f && x_axis >= -0.2f)))
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
