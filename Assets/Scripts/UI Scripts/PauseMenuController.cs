using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject topPauseMenu;
    public GameObject settingsMenu;

    private bool isPaused = false;

    void Awake()
    {
        isPaused = false;
    }

    public void OnPauseButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        //pause while playing 
        if (!isPaused)
        {
            TogglePause();
        }
        //unpause only if player is looking at the top pause menu
        else
        {
            if (settingsMenu.activeSelf) TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            topPauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            topPauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    

    public void RestartCurrentMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
