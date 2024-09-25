using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform normalPlayerGameObject;
    [SerializeField] private Transform ghostPlayerGameObject;

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
        else if (parentTree.passageState == ParentTreePassage.Available.open) // ��� ���� ���°� open
        {
            Transform MoveToExit = FindingExitPosition();

            if (MoveToExit == null) return;
            
            normalPlayerGameObject.transform.position = MoveToExit.position; // �ݴ��� �ⱸ�� �̵�
            ghostPlayerGameObject.transform.position = MoveToExit.position;
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
