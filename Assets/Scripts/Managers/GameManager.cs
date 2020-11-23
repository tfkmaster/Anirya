using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager GMInstance;
    private string checkpointSceneName;
    private Vector3 checkpointPosition;
    private GameObject LastCheckpoint;
    public GameObject Player;
    private GameObject myPlayer;
    // Start is called before the first frame update

    void Awake()
    {
        if(GMInstance == null)
        {
            GMInstance = this;
            DontDestroyOnLoad(GMInstance);
            myPlayer = Instantiate(Player);
            DontDestroyOnLoad(myPlayer);
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
        if (checkpointSceneName != "")
        {
            SceneManager.LoadScene(checkpointSceneName);
            myPlayer.transform.position = checkpointPosition;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpdateCheckpoint(GameObject checkpoint)
    {
        if (checkpoint.GetComponent<CheckpointController>())
        {
            if (LastCheckpoint && (LastCheckpoint.transform.position != checkpoint.GetComponent<CheckpointController>().MyPosition.position || checkpointSceneName != checkpoint.GetComponent<CheckpointController>().MyScene.name))
            {
                LastCheckpoint.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
            checkpointPosition = checkpoint.GetComponent<CheckpointController>().MyPosition.position;
            checkpointSceneName = checkpoint.GetComponent<CheckpointController>().MyScene.name;
            checkpoint.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            LastCheckpoint = checkpoint;
        }
    }

    //Launches the loading scene method with the according parameter
    public void LoadScene(string sceneName, string spawnPointName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, spawnPointName));
    }

    // Asynchronous Scene loading, so that methods get executed after the entire scene is loaded
    private IEnumerator LoadSceneAsync(string sceneName, string spawnPointName)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        //Move the player to the adequate spawn point
        myPlayer.transform.position = GameObject.FindGameObjectWithTag(spawnPointName).transform.position;
    }
}
