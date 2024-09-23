using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform Player;
    [SerializeField] private Item item;
    [SerializeField] private GameObject spiderWeb;
    private ParentTreePassage parentTree;

    private void Start()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform != null ) parentTree = parentTransform.GetComponent<ParentTreePassage>();
        //Debug.Log(parentTree.passageState);
    }
    public void Interact()
    {
        Debug.Log("current state : " + parentTree.passageState);

        if (parentTree.passageState == ParentTreePassage.Available.closed && parentTree.invetory != null)
        {
            if (parentTree.invetory.FindItem(item))
            {
                parentTree.invetory.UseItem(item);
                parentTree.passageState = ParentTreePassage.Available.open;
                if (spiderWeb != null) spiderWeb.SetActive(false);
                Debug.Log("now tree passage is open");
            }
        }
        else if (parentTree.passageState == ParentTreePassage.Available.open) // 통로 나무 상태가 open
        {
            Transform MoveToExit = FindingExitPosition();

            if (MoveToExit != null) Player.transform.position = MoveToExit.position; // 반대편 출구로 이동
            else Debug.Log("Exit is NULL");
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
