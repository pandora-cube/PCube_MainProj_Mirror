using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private InputAction pauseButton;
    [SerializeField] private GameObject canvas;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseButton.Enable();
    }
    private void OnDisable()
    {
        pauseButton.Disable();
    }
    void Start()
    {
        pauseButton.performed += ctx => Pause();
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            canvas.SetActive(true);
        }

        if (!isPaused)
        {
            Time.timeScale = 1f;
            canvas.SetActive(false);
        }
    }
}
