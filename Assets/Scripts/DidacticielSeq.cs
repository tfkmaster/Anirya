using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidacticielSeq : MonoBehaviour
{
    public List<GameObject> TutoGuides = default;
    private float timer = 0f;
    private float BUTTERFLY_CINEMATIC_TIME = 7.5f;

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

        if(timer >= BUTTERFLY_CINEMATIC_TIME)
        {
            Anirya.GetComponentInChildren<Animator>().SetBool("interacting", false);
            Anirya.GetComponent<CharacterMovement>().Interacting = false;
        }

        if (tutorial_step == 0 && (Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") >= 0.2f))
        {
            tutorial_step = 2;
        }

        if(tutorial_step == 0 && timer >= 10f)
        {
            StartCoroutine(FadeCanvas(TutoGuides[0].GetComponent<CanvasGroup>(), 0f, 1f, 1.2f, true));
            tutorial_step++;
        }
        if(tutorial_step == 1 && timer >= 10f && (Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") >= 0.2f))
        {
            StartCoroutine(FadeCanvas(TutoGuides[0].GetComponent<CanvasGroup>(), 1f, 0f, 0.8f, false));
            tutorial_step++;
        }
        if(tutorial_step == 2 && Anirya.transform.position.x >= 60f)
        {
            StartCoroutine(FadeCanvas(TutoGuides[1].GetComponent<CanvasGroup>(), 0f, 1f, 1.2f, true));
            tutorial_step++;
        }
        if(tutorial_step == 3 && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(FadeCanvas(TutoGuides[1].GetComponent<CanvasGroup>(), 1f, 2f, 0.8f, false));
            tutorial_step++;
        }
    }

    public static IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration, bool activate)
    {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        canvas.alpha = startAlpha;

        if (activate)
        {
            canvas.gameObject.SetActive(true);
        }

        while(Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if(startAlpha > endAlpha)
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
