using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsUI : MonoBehaviour
{
    public GameObject PauseUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseUI.active)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        PauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
