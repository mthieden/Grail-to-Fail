using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit(); 
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
