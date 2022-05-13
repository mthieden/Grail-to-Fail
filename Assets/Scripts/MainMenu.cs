using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameOverButton()
    {
        SceneManager.LoadScene("Menu");
        if(Player.instance == null)
        {
            Debug.Log("GAME IS OVER AND PLAYER IS NULL");
        }
        else
        {
            Debug.Log("GAME IS OVER, PLAYER IS NOT NULL");
        }
    }
}
