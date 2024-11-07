using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnterTrigger : PlayerTutorial
{   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thisTutoPlaying && collision.CompareTag("Player"))
        {
            PlayTutorial();
        }
    }
}
