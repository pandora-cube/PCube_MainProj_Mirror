using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    private DialogSystem dialogSystem => DialogSystem.instance;
    public bool isTutoPlaying = false;
    private void Awake()
    {
        #region singleton
        if (instance == null) instance = this;
        else Destroy(instance);
        #endregion
    }

    public void StartTutorialDialog()
    {
        dialogSystem.StartDialog();
        isTutoPlaying = false;
    }
}
