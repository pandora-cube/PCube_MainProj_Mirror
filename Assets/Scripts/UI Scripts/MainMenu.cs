using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public RectTransform targetUIElement;
    public float targetScale = 1.5f; // Scale factor when zoomed in
    public float zoomSpeed = 5f; // Speed of the zoom
    private Vector3 originalScale;
    private bool isZoomingIn = false;

    void Start()
    {
        if (targetUIElement == null)
            Debug.LogError("Target UI Element is not assigned!");

        originalScale = targetUIElement.localScale;
    }

    void Update()
    {
        if (isZoomingIn)
        {
            targetUIElement.localScale = Vector3.Lerp(targetUIElement.localScale, originalScale * targetScale, Time.deltaTime * zoomSpeed);
            if (Vector3.Distance(targetUIElement.localScale, originalScale * targetScale) < 0.01f)
            {
                targetUIElement.localScale = originalScale * targetScale;
                isZoomingIn = false;
                MoveToNextScene();
            }
        }
    }

    public void OnZoomButtonClicked()
    {
        isZoomingIn = true;
    }

    public void ResetZoom()
    {
        targetUIElement.localScale = originalScale;
    }

    private void MoveToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
