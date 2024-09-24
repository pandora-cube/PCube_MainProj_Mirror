using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    const int FORST_STAGE_MAP1 = 2;
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
