using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager GMInstance;
    public Vector2 LastCheckpointPosition = new Vector2(0,0);
    public string LastCheckpointSceneName;
    public GameObject Player;
    // Start is called before the first frame update

    void Awake()
    {
        if(GMInstance == null)
        {
            GMInstance = this;
            DontDestroyOnLoad(GMInstance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCheckpoint()
    {
        if (LastCheckpointSceneName != "")
        {
            SceneManager.LoadScene(LastCheckpointSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void LoadScene()
        {

        }
}
