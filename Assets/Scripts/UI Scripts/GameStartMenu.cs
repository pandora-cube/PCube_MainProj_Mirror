using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    const int FORST_STAGE_MAP1 = 2;

    [SerializeField] private GameObject popupPanel;
    public void OnStartCliked()
    {
        if (ProgressData.Instance.playerData.currentChapter == 1 && ProgressData.Instance.playerData.currentStage == 1)
        {
            StartFromBeginning();
        }
        else
        {
            popupPanel.SetActive(true);
        }

    }

    public void StartFromBeginning()
    {
        SceneManager.LoadScene(FORST_STAGE_MAP1);
    }

    public void StartToContinue()
    {
        ProgressData.Instance.LoadData();
        SceneManager.LoadScene(FORST_STAGE_MAP1);
    }
}
