using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuto_002_Manager : MonoBehaviour
{
    private ScenesManager PersistentDatas = default;
    private Player player = default;
    [SerializeField]
    private GameObject trunk = default;
    [SerializeField]
    private GameObject tallam_shadows = default;
    [SerializeField]
    private CanvasGroup TutoGuide = default;
    private int display_guide = 0;

    void Awake()
    {
        PersistentDatas = GameObject.FindGameObjectWithTag("PersistentDatas").GetComponent<ScenesManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        if (PersistentDatas.tuto_002.TrunkIsBroken)
        {
            Destroy(trunk);
        }

        if (!PersistentDatas.tuto_005.FirstWolfSlayed)
        {
            tallam_shadows.GetComponent<ParticleSystem>().Play();
            tallam_shadows.GetComponentInChildren<Collider2D>().isTrigger = false;
        }
    }

    void Update()
    {
        if (!PersistentDatas.tuto_002.TrunkIsBroken && !trunk)
        {
            PersistentDatas.tuto_002.TrunkIsBroken = true;
        }

        if (!PersistentDatas.tuto_002.XAttackDisplay)
        {
            if (display_guide == 1)
            {
                StopAllCoroutines();
                StartCoroutine(FadeCanvas(TutoGuide, 0f, 1f, 1.2f, true));
                ++display_guide;
            }
            else if (display_guide == 2 && Input.GetKeyDown("joystick button 2"))
            {
                ++display_guide;
                StopAllCoroutines();
                StartCoroutine(FadeCanvas(TutoGuide, 1f, 0f, 1.2f, true));
                PersistentDatas.tuto_002.XAttackDisplay = true;
            }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && display_guide == 0)
        {
            Debug.Log("Dedans");
            display_guide = 1;
        }
    }
}
