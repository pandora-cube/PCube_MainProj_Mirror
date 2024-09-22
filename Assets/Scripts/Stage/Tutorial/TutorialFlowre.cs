using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlowre : MonoBehaviour
{
    [SerializeField] private Tutorial tutorial;
    private void OnDestroy()
    {
        tutorial.Tuto4_flowre();
    }
}
