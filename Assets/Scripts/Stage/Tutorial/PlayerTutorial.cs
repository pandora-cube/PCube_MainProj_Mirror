using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] Tutorial tuto;
    [SerializeField] string funcName;
    private bool tutoPlay = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tutoPlay && collision.CompareTag("Player"))
        {
            tutoPlay = false;
            tuto.StartCoroutine(funcName);
        }
    }
}
