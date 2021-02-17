using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    private GameManager GM;
    public Scene MyScene;
    public Transform MyPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameManager"))
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        MyScene = SceneManager.GetActiveScene();
        MyPosition = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGM(GameManager gm)
    {
        GM = gm;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GM.UpdateCheckpoint(this.gameObject);
        }
    }
}
