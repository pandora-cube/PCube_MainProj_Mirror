using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerNormalObject;
    [SerializeField] private GameObject playerGhostObject;
    [SerializeField] private PlayerController playerController;


    // Update is called once per frame
    void Update()
    {
        if (playerController.isNormal) transform.position = new Vector3(playerNormalObject.transform.position.x, playerNormalObject.transform.position.y, transform.position.z);
        else if (playerController.isGhost) transform.position = new Vector3(playerGhostObject.transform.position.x, playerGhostObject.transform.position.y, transform.position.z);

    }
}
