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

    public void TreePassageCameraMoving(Vector3 startPosition, Vector3 targetPosition, int startIndex)
    {
        Vector3 adjustedStartPosition = new Vector3(startPosition.x, startPosition.y, -10);
        //Vector3 adjustedTargetPosition = new Vector3(targetPosition.x, targetPosition.y, -10);
        cameraTrack.ActivateMyDollyTrack(adjustedStartPosition);
    }
}
