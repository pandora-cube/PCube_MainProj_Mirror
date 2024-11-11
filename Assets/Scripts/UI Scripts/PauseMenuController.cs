using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject topPauseMenu;
    public GameObject[] subMenus;
    private bool isPaused = false;
    private bool isSubMenuActivated = false;
    private int activatedSubMenuIdx = 0;
    void Awake()
    {
        isPaused = false;
        isSubMenuActivated = false;
        activatedSubMenuIdx = 0;
    }

    public void OnPauseButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        //pause while playing 
        if (!topPauseMenu.activeSelf)
        {
            TogglePause();
        }
        //go back while in pause menu
        else
        {
            FindActivatedSubMenu();
            Debug.Log(isPaused);
            if (isSubMenuActivated) CloseActivatedSubMenu(); //if the player is looking at a sub menu
            else TogglePause(); // if the player is at the top pause menu
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

    private void FindActivatedSubMenu()
    {
        //iterate through the array of submenus
        //if submenu is active, set the bool as true and keep the index of activated sub menu.
        for (int i = 0; i < subMenus.Length; ++i)
        {
            if (!subMenus[i].activeSelf) continue;

            isSubMenuActivated = true;
            activatedSubMenuIdx = i;
        }
    }

    private void CloseActivatedSubMenu()
    {
        subMenus[activatedSubMenuIdx].SetActive(false); //turn off the submenu
        topPauseMenu.SetActive(true); //turn on the top pause menu
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
