using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnterTrigger : PlayerTutorial
{   
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thisTutoPlaying && collision.CompareTag("Player"))
        {
            PlayTutorial();
        }
    }
}
