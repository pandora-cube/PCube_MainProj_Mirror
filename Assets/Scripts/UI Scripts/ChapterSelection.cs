using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelection : MonoBehaviour
{
    [SerializeField] private Image[] chapterSelectionImages;
    private int index;
    private int numberOfImages;

    private MenuHeirarchyController menuHeirarchyController;

    void Awake()
    {
        menuHeirarchyController = GetComponent<MenuHeirarchyController>();
    }
    
    void Start()
    {
        numberOfImages = chapterSelectionImages.Length;
        SetImageActive(index);
    }

    public void SetIndex(int idx)
    {
        index = idx;
        SetImageActive(index);
    }

    public void ShowNextImage()
    {
        if (index == numberOfImages - 1)
        {
            SceneManager.LoadScene("MainMenu");
        }  
        else
        {
            if (index == numberOfImages - 2) menuHeirarchyController.PopStack();
            index++;
            SetImageActive(index);
        }
    }
    
    public void ShowPreviousImage()
    {
        if (index == 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (index == 1) menuHeirarchyController.PopStack();
            index--;
            SetImageActive(index);
        } 
    }

    private void SetImageActive(int idx)
    {
        chapterSelectionImages[idx].gameObject.SetActive(true);

        for (int i = 0; i < numberOfImages; ++i)
        {
            if (i == idx) continue;
            chapterSelectionImages[i].gameObject.SetActive(false);
        }
    }
}
