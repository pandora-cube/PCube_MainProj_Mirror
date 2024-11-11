using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject topPauseMenu;
    public GameObject settingsMenu;
    public GameObject[] subMenus;

    private bool isPaused = false;
    int activatedSubMenuIdx;

    void Awake()
    {
        isPaused = false;
        activatedSubMenuIdx = -1;
    }

    public void OnPauseButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        //pause while playing 
        if (!isPaused)
        {
            TogglePause();
        }
        else
        {
            bool isAtSettingsSubMenu = IsAllSubMenusClosed();

            //player is not looking at any of the settings submenu
            if (!isAtSettingsSubMenu)
            {
                //unpause only if player is looking at the top pause menu
                if (!settingsMenu.activeSelf && isPaused) TogglePause();

                //player is only looking at the top settings menu. Press esc to toggle pause menu
                else if (settingsMenu.activeSelf && isPaused)
                {
                    settingsMenu.SetActive(false);
                    topPauseMenu.SetActive(true);
                }
            }
            else
            {
                //player is in the settings submenu
                Debug.Log("Yes~");
                CloseActivatedSubMenu();
            }
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

    private bool IsAllSubMenusClosed()
    {
        //iterate through the array of submenus
        //if submenu is active, keep the index of activated sub menu and return true
        for (int i = 0; i < subMenus.Length; ++i)
        {
            if (subMenus[i].activeSelf)
            {
                activatedSubMenuIdx = i;
                return true;
            }
        }

        return false;
    }

    private void CloseActivatedSubMenu()
    {
        subMenus[activatedSubMenuIdx].SetActive(false); //turn off the submenu
        settingsMenu.SetActive(true); //turn on the top pause menu
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
