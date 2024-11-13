using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCameraPosition : MonoBehaviour
{
    [SerializeField] Transform obj;
    [SerializeField] int CameraSize = 20;
    PlayerCameraController cameraController => PlayerCameraController.instance;

    public void ActiveCameraProduction()
    {
        if (obj == null) obj = transform;

        Vector3 targetTransform = new Vector3(obj.position.x, obj.position.y, -10);
        cameraController.SetProductionCamera(targetTransform, CameraSize);
        cameraController.StartProductionCamera();
    }
    

    public void ExitCameraProduction()
    {
        cameraController.ReturnCameraPosition();
    }
}
