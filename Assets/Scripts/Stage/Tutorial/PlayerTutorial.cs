using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    protected DialogSystem dialogSystem => DialogSystem.instance;
    protected CinemachineDollyTrack dollyTrack;
    protected bool thisTutoPlaying = false;

    private void Start()
    {
        dollyTrack = GetComponent<CinemachineDollyTrack>();
    }

    public virtual void PlayTutorial()
    {
        thisTutoPlaying = true;
        StartCoroutine(StartTutorialDialog());
    }

    public IEnumerator StartTutorialDialog()
    {
        if (dollyTrack != null) dollyTrack.ActivateMyDollyTrack();

        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        
        ExitTutorial();
    }

    public virtual void ExitTutorial()
    {
        if (dollyTrack != null) dollyTrack.ExitCameraProduction();
    }
}
