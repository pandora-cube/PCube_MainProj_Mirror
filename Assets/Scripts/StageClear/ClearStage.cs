using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearStage : MonoBehaviour
{
    private BoxCollider2D collid;
    [SerializeField] string nextScnene;
    private void Start()
    {
        collid = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collid.isTrigger && collision.CompareTag("Player")) SceneManager.LoadScene(nextScnene);
    }
}
