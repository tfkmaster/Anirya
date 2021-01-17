using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField]
    private Image circle_progress_bar = default;
    [SerializeField]
    private Image b_button = default;
    [SerializeField]
    private Image b_button_background = default;

    [SerializeField]
    private VideoPlayer cinematic = default;

    private float delay = 2.5f;
    private bool decrease_bar = false;

    private bool game_launched = false;
    [SerializeField]
    private Canvas TransitionCanvas = default;

    void Start()
    {
        cinematic.loopPointReached += LaunchGame;
    }

    void Update()
    {
        if (Input.GetKey("joystick button 1"))
        {
            decrease_bar = false;
            circle_progress_bar.fillAmount += 1 / (delay / Time.deltaTime);

            b_button.gameObject.SetActive(true);
            b_button_background.gameObject.SetActive(true);
        }

        if (Input.GetKeyUp("joystick button 1") && circle_progress_bar.fillAmount >= 0f)
        {
            decrease_bar = true;
        }

        if (decrease_bar)
        {
            circle_progress_bar.fillAmount -= 1 / (delay / Time.deltaTime);
        }

        if (circle_progress_bar.fillAmount <= 0f)
        {
            b_button.gameObject.SetActive(false);
            b_button_background.gameObject.SetActive(false);
        }

        if (circle_progress_bar.fillAmount >= 1f)
        {
            game_launched = true;
        }

        if (game_launched)
        {
            circle_progress_bar.gameObject.SetActive(false);
            b_button.gameObject.SetActive(false);
            b_button_background.gameObject.SetActive(false);
            StartCoroutine(FadeCanvas(TransitionCanvas.GetComponent<CanvasGroup>(), 0f, 1f, 2f, true));
            game_launched = false;
        }

        if (TransitionCanvas.GetComponent<CanvasGroup>().alpha >= 1f)
        {
            StopAllCoroutines();
            SceneManager.LoadScene("1");
        }
    }

    void LaunchGame(VideoPlayer vp)
    {
        game_launched = true;
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
