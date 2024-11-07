using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    private DialogSystem dialogSystem => DialogSystem.instance;
    private CinemachineDollyTrack dollyTrack;
    bool thisTutoPlaying = false;

    private void Start()
    {
        dollyTrack = GetComponent<CinemachineDollyTrack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thisTutoPlaying && collision.CompareTag("Player"))
        {
            thisTutoPlaying = true;
            StartCoroutine(StartTutorialDialog());
        }
    }

    public IEnumerator StartTutorialDialog()
    {
        if (dollyTrack != null) dollyTrack.ActivateMyDollyTrack();
        yield return dialogSystem.StartCoroutine(dialogSystem.DialogProgress());
        if (dollyTrack != null) dollyTrack.ExitCameraProduction();
    }
}
