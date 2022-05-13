using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Completed;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerHealth;


    public Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;

    void Awake()
    {

        Debug.Log("MANAGER WAKING UP");

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
       // if (Player.instance == null) { Debug.Log("PLAYER IS NULL???!!?"); }
        //InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log("STARTING MANAGER");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("MAIN SCENE LOADED");

        //level++;
        if (scene.name == "Main")
        {
            InitGame();
        }
    }

    void InitGame()
    {
        Debug.Log("INITGAME, playerHleath: " + playerHealth);
        if(level > 1) Player.instance.Health = playerHealth;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void NextLevel()
    {
        playerHealth = Player.instance.Health;
        level++;
        Debug.Log("SAVED PLAYER HEALTH IN MANGAGER: " + playerHealth);
        SceneManager.LoadScene("Main");
    }
    

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        //playerHealth = 100;
        level = 1;

        //levelText.text = "you reached level" + level;
        //levelImage.SetActive(true);
        //enabled = false;
    }
}
