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
    public float TransitionTime = 1;
    private bool isLoading = false;
    private Animator sceneTransitionAnimator;

    public bool alimMet = false;
    
    public bool isPaused = default;

    //UI Management
    public GameObject UIManager;
    private GameObject UIManagerInstance;

    //SceneTransition
    public GameObject LevelLoader;
    private GameObject LevelLoaderInstance;

    void Awake()
    {
        if(GMInstance == null)
        {
            GMInstance = this;
            DontDestroyOnLoad(GMInstance);
            myPlayer = Instantiate(Player,this.transform.position,new Quaternion(0,0,0,0));
            UIManagerInstance = Instantiate(UIManager, this.transform.position, new Quaternion(0, 0, 0, 0));
            LevelLoaderInstance = Instantiate(LevelLoader, this.transform.position, new Quaternion(0, 0, 0, 0));

            //Do put in a scene Manager
            GameObject.FindGameObjectWithTag("FollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = myPlayer.transform;
            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(UIManagerInstance);
            DontDestroyOnLoad(LevelLoaderInstance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isPaused = false;
        sceneTransitionAnimator = LevelLoaderInstance.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        changeSceneHelper();
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
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName, spawnPointName));
        }
    }

    // Asynchronous Scene loading, so that methods get executed after the entire scene is loaded
    private IEnumerator LoadSceneAsync(string sceneName, string spawnPointName)
    {
        isLoading = true;
        sceneTransitionAnimator.SetBool("end", false);
        sceneTransitionAnimator.SetBool("start",true);
        yield return new WaitForSeconds(TransitionTime);
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        //Move the player to the adequate spawn point
        myPlayer.transform.position = GameObject.FindGameObjectWithTag(spawnPointName).transform.position;
        if (GameObject.FindGameObjectWithTag("Alim") && alimMet)
        {
            GameObject.FindGameObjectWithTag("Alim").transform.position = GameObject.FindGameObjectWithTag(spawnPointName).transform.position;
        }
        sceneTransitionAnimator.SetBool("start", false);
        sceneTransitionAnimator.SetBool("end", true);
        isLoading = false;
    }

    //pause the game
    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    //resume the game
    void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    //allows the UIManager to display the menu
    public void SetPause() 
    {
        if (isPaused)
        {
            if (UIManagerInstance.GetComponent<UIManager>().controlScreenOn)
            {
                UIManagerInstance.GetComponent<UIManager>().back_to_pause_menu_button_action();
            }
            else
            {
                UIManagerInstance.GetComponent<UIManager>().DisplayPauseMenu(false);
                ResumeGame();
            }
        }
        else 
        {
            UIManagerInstance.GetComponent<UIManager>().DisplayPauseMenu(true);
            PauseGame();
        }
    }

    //exit the game
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void SendPlayerStatsToUIManager(int maxHealth, int actualHealth, float maxHeat, float actualHeat)
    {
        UIManagerInstance.GetComponent<UIManager>().SendPlayerStatsToPlayerStatsManager(maxHealth, actualHealth, maxHeat, actualHeat);
    }

    void changeSceneHelper()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene("tuto_001", "sp_tuto001_01");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene("tuto_002", "sp_tuto002_01");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadScene("tuto_004", "sp_tuto004_01");
        }
    }
}
