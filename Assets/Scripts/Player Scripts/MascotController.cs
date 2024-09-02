using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascotController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] GameObject playerNormalObject;
    [SerializeField] GameObject playerGhostObject;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        if (playerController.isNormal) gameObject.transform.position = playerNormalObject.transform.position + new Vector3(-6f, 0f, 0f);
        else if (playerController.isGhost) gameObject.transform.position = playerGhostObject.transform.position + new Vector3(-6f, 0f, 0f);
        
    }
}
