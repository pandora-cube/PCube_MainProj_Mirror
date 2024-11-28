using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParentTreePassage : MonoBehaviour
{
    public enum Available { open, closed }
    public Available passageState;
    private CinemachineDollyTrack cameraTrack;

    private void Awake()
    {
        cameraTrack = GetComponent<CinemachineDollyTrack>();
    }

    public void TreePassageCameraMoving(int cameraPathIndex)
    {
        cameraTrack.ActivateMyDollyTrack(cameraPathIndex);
    }

    public void TreePassageCameraStopped()
    {
        cameraTrack.ExitProductionCamera();
    }

    public void SettingCamera(Vector3 startPosition)
    {
        Vector3 adjustedStartPosition = new Vector3(startPosition.x, startPosition.y, -10);
        cameraTrack.TurnProductionCamera(adjustedStartPosition);
    }
}
