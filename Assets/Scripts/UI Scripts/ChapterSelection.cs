using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelection : MonoBehaviour
{
    [SerializeField] private Image[] chapterSelectionImages;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton; 
    private int index;
    private int numberOfImages;

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
            if (index == numberOfImages - 2)  nextButton.GetComponent<Image>().color = Color.black;
            else nextButton.GetComponent<Image>().color = Color.white;
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
            if (index == 1) previousButton.GetComponent<Image>().color = Color.black;
            else previousButton.GetComponent<Image>().color = Color.white;
            index--;
            SetImageActive(index);
        } 
    }

    private void SetImageActive(int idx)
    {
        if (idx == 0) previousButton.GetComponent<Image>().color = Color.black;
        else previousButton.GetComponent<Image>().color = Color.white;

        if (idx == numberOfImages - 1) nextButton.GetComponent<Image>().color = Color.black;
        else nextButton.GetComponent<Image>().color = Color.white;

        chapterSelectionImages[idx].gameObject.SetActive(true);

        for (int i = 0; i < numberOfImages; ++i)
        {
            if (i == idx) continue;
            chapterSelectionImages[i].gameObject.SetActive(false);
        }
    }
}
