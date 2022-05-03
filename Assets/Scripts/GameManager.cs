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
    public int playerFoodPoints;


    public Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private static int testasd = 0;

    void Awake()
    {
        Debug.Log("Waking up ...");

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        //InitGame();
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        //level++;
        if(scene.name == "Main")
        {
            InitGame();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //level++;
        //InitGame();
    }

    void InitGame()
    {
        testasd++;
        Debug.Log("INITGAME" + testasd);
        doingSetup = true;

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
        doingSetup = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void NextLevel()
    {
        if(Player.instance == null) { Debug.Log("PLAYER IS NULL???!!?"); }
        playerFoodPoints = Player.instance.Food;
        level++;
        SceneManager.LoadScene("Main");
    }
    

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        playerFoodPoints = 100;
        Debug.Log("RESETTING FOOD: " + playerFoodPoints);
        level = 1;
        //levelText.text = "you reached level" + level;
        //levelImage.SetActive(true);
        //enabled = false;
    }
}
