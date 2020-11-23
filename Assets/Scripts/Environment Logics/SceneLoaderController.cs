using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderController : MonoBehaviour
{
    private GameManager GM;
    public string SceneToGoTo;
    public string SpawnPointToGoTo;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GM.LoadScene(SceneToGoTo, SpawnPointToGoTo);
        }
    }

}
