using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidacticielSeq : MonoBehaviour
{
    public List<GameObject> TutoGuides = default;
    private float timer = 0f;

    private float jump_timer = 0f;

    private GameObject Anirya = default;

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

        if(timer >= 8f)
        {
            TutoGuides[0].SetActive(true);
        }
        if(timer >= 13f)
        {
            TutoGuides[0].SetActive(false);
        }
        if(Anirya.transform.position.x >= 56f && jump_timer <= 12f)
        {
            jump_timer += Time.deltaTime;
            TutoGuides[1].SetActive(true);
        }
        if(jump_timer >= 12f)
        {
            TutoGuides[1].SetActive(false);
        }
    }
}
