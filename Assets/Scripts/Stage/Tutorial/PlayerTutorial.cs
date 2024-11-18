using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    protected DialogSystem dialogSystem => DialogSystem.instance;
    //protected CinemachineDollyTrack dollyTrack;
    protected MovingCameraPosition cameraPosition;
    protected bool thisTutoPlaying = false;

    private void Start()
    {
        cameraPosition = GetComponent<MovingCameraPosition>();
    }

    public virtual void PlayTutorial()
    {
        thisTutoPlaying = true;
        StartCoroutine(StartTutorialDialog());
    }

    public virtual IEnumerator StartTutorialDialog()
    {
        if (cameraPosition != null) cameraPosition.ActiveCameraProduction();

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        
        ExitTutorial();
    }

    public virtual void ExitTutorial()
    {
        if (cameraPosition != null) cameraPosition.ExitCameraProduction();
    }
}
