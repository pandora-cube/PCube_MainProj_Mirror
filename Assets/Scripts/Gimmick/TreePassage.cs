using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform normalPlayerGameObject;
    [SerializeField] private Transform ghostPlayerGameObject;

    [SerializeField] private Item item;
    [SerializeField] private GameObject spiderWeb;

    public Inventory invetory => Inventory.instance;

    ParentTreePassage parentTree;
    private void Start()
    {
        parentTree = transform.parent.GetComponent<ParentTreePassage>();
    }
    public void Interact()
    {
        Debug.Log("current state : " + parentTree.passageState);

        if (parentTree.passageState == ParentTreePassage.Available.closed && invetory != null)
        {
            if (invetory.FindItem(item))
            {
                invetory.UseItem(item);
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

    public Transform GetTransform()
    {
        return transform;
    }
}
