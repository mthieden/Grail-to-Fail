using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton()
    {
        SceneManager.LoadScene("GameOver");
        //Application.Quit();
        //Debug.Log("Closed game");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameOverButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
