using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private PlayerInput playerInput;
    private InputActionMap gameplayActionMap;
    private InputActionMap UIActionMap;
    private bool isPaused = false;

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        gameplayActionMap = playerInput.actions.FindActionMap("PlayerActions"); 
        UIActionMap = playerInput.actions.FindActionMap("UI Actions");
    }
    
    public void OnPause(InputAction.CallbackContext ctx)
    {
        TooglePause();
    }

    public void TooglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            gameplayActionMap.Enable();
            UIActionMap.Disable();
        }
        else
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            gameplayActionMap.Disable();
            UIActionMap.Enable();
        }

        isPaused = !isPaused;
    }

    public void RestartCurrentMap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
