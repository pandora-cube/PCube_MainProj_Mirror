using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform Player;
    public void Interact(Transform interactorTransform)
    {
        Transform MoveToExit = FindingExitPosition();
        //Debug.Log(MoveToExit.gameObject.name);
        if (MoveToExit != null)
        {
            Player.transform.position = MoveToExit.position;
            //Debug.Log("Player Transform : " + Player.transform.position + " Exit Transform " + MoveToExit.transform.position);
        }
    }

    Transform FindingExitPosition()
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            foreach (Transform sibling in parent)
            {
                if (sibling.gameObject != gameObject) return sibling.gameObject.transform;
            }
        }
        return null;
    }

    public string GetInteractText()
    {
        string text = "Interact";
        return text;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
