using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidacticielSeq : MonoBehaviour
{
    public List<GameObject> TutoGuides = default;
    private float timer = 0f;

    private GameObject Anirya = default;

    private int tutorial_step = 0;

    // Start is called before the first frame update
    void Start()
    {
        Anirya = GameObject.FindGameObjectWithTag("Player");
        foreach(GameObject tutos in TutoGuides)
        {
            tutos.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (tutorial_step == 0 && (Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") >= 0.2f))
        {
            tutorial_step = 2;
        }

        if(tutorial_step == 0 && timer >= 10f)
        {
            TutoGuides[0].SetActive(true);
            tutorial_step++;
        }
        if(tutorial_step == 1 && timer >= 10f && (Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") >= 0.2f))
        {
            TutoGuides[0].SetActive(false);
            tutorial_step++;
        }
        if(tutorial_step == 2 && Anirya.transform.position.x >= 66f)
        {
            TutoGuides[1].SetActive(true);
            tutorial_step++;
        }
        if(tutorial_step == 3 && Input.GetButtonDown("Jump"))
        {
            TutoGuides[1].SetActive(false);
            tutorial_step++;
        }
    }
}
