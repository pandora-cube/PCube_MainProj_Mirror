using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    private TutorialManager tutorial => TutorialManager.instance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tutorial.isTutoPlaying && collision.CompareTag("Player"))
        {
            tutorial.isTutoPlaying = true;
            tutorial.StartTutorialDialog();
        }
    }
}
