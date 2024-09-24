using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject[] subMenus; // Array of submenus
    private GameObject currentMenu;
    private PlayerInput playerInput;
    private InputActionMap gameplayActionMap;
    private InputActionMap UIActionMap;
    [SerializeField] private bool isPaused = false;

    private Stack<GameObject> menuStack = new Stack<GameObject>();

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        gameplayActionMap = playerInput.actions.FindActionMap("PlayerActions"); 
        UIActionMap = playerInput.actions.FindActionMap("UI Actions");
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
             if (isPaused && menuStack.Count > 0)
            {
                CloseSubMenu(); // Close submenu if one is open
            }
            else
            {
                TogglePause();  // Toggle pause menu if no submenus are open
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (!isPaused)
        {
            CloseAllMenus();
            Time.timeScale = 1f;
            gameplayActionMap.Enable();
            UIActionMap.Disable();
        }
        else
        {
            Time.timeScale = 0f;
            OpenSubMenu(pausePanel); // Open the pause menu first
            gameplayActionMap.Disable();
            UIActionMap.Enable();
        }
    }

    // Opens a submenu and hides the current menu
    public void OpenSubMenu(GameObject subMenu)
    {
        if (currentMenu != null)
        {
            currentMenu.SetActive(false); // Hide the current menu (either pause or another sub-menu)
            menuStack.Push(currentMenu);  // Push the current menu onto the stack
        }

        subMenu.SetActive(true);          // Show the new sub-menu
        currentMenu = subMenu;            // Set it as the current menu
    }

    // Closes the current sub-menu and returns to the previous one
    public void CloseSubMenu()
    {
        if (menuStack.Count > 0)          // Check if there are previous menus
        {
            currentMenu.SetActive(false); // Close the current menu
            currentMenu = menuStack.Pop(); // Pop the previous menu from the stack
            currentMenu.SetActive(true);  // Show the previous menu
        }
        else
        {
            TogglePause();                // If no previous menus, just unpause the game
        }
    }

    // Closes all menus and resumes the game
    public void CloseAllMenus()
    {
        while (menuStack.Count > 0)
        {
            menuStack.Pop().SetActive(false); // Deactivate and clear all menus
        }
        pausePanel.SetActive(false); // Always close the pause panel
        currentMenu = null;
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
