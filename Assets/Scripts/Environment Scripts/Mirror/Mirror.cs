using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public void Interact()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.Transform();
        Debug.Log("Interacted");
    }
}
