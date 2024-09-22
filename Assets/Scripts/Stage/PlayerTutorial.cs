using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] Tutorial tuto;
    [SerializeField] string funcName;
    private bool canPlaying = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPlaying && collision.CompareTag("Player"))
        {
            canPlaying = false;
            tuto.StartCoroutine(funcName);
        }
    }
}
