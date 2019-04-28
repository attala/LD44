using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Tutorial tut = GetComponent<Tutorial>();
        if (tut == null)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
