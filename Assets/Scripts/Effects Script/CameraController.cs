using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject normalPlayerObject;
    [SerializeField] private GameObject ghostPlayerObject;
    private PlayerController playerController;
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isNormal) transform.position = new Vector3(normalPlayerObject.transform.position.x, normalPlayerObject.transform.position.y, -10f);
        else if (playerController.isGhost) transform.position = new Vector3(ghostPlayerObject.transform.position.x, ghostPlayerObject.transform.position.y, -10f);
    }
}
