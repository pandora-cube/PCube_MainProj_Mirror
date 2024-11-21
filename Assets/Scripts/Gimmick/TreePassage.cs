using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : ParentTreePassage, IInteractable
{
    [SerializeField] private Transform normalPlayerGameObject;
    [SerializeField] private Transform ghostPlayerGameObject;

    [SerializeField] private Item item;
    [SerializeField] private GameObject spiderWeb;

    private void Start()
    {

    }
    public void Interact()
    {
        Debug.Log("current state : " + passageState);

        if (passageState == ParentTreePassage.Available.closed && invetory != null)
        {
            if (invetory.FindItem(item))
            {
                invetory.UseItem(item);
                passageState = ParentTreePassage.Available.open;
                if (spiderWeb != null) spiderWeb.SetActive(false);
                Debug.Log("now tree passage is open");
            }
        }
        else if (passageState == ParentTreePassage.Available.open) // ��� ���� ���°� open
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

    public Transform GetTransform()
    {
        return transform;
    }
}
