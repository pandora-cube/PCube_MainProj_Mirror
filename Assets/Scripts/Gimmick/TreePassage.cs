using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePassage : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform normalPlayerGameObject;
    [SerializeField] private Transform ghostPlayerGameObject;

    [SerializeField] private Item item;
    [SerializeField] private GameObject spiderWeb;
    private MovingCameraPosition cameraPosition;
    public Inventory invetory => Inventory.instance;
    int passageHoleIndex;

    ParentTreePassage parentTree;
    private void Start()
    {
        parentTree = transform.parent.GetComponent<ParentTreePassage>();
        cameraPosition = GetComponent<MovingCameraPosition>();

        if (transform.gameObject.name == "top hole") passageHoleIndex = 1;
        else passageHoleIndex = 0;
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
            StartCoroutine(MovingProduction(MoveToExit));
        }
    }

    IEnumerator MovingProduction(Transform MoveToExit)
    {
        parentTree.SettingCamera(transform.position);

        yield return new WaitForSeconds(1f);
        parentTree.TreePassageCameraMoving(passageHoleIndex);

        yield return new WaitForSeconds(1f);
        normalPlayerGameObject.transform.position = MoveToExit.position; // �ݴ��� �ⱸ�� �̵�
        ghostPlayerGameObject.transform.position = MoveToExit.position;

        yield return new WaitForSeconds(1f);
        parentTree.TreePassageCameraStopped();
    }

    Transform FindingExitPosition()
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            foreach (Transform sibling in parent)
            {
                if (sibling.gameObject != this.gameObject && sibling.gameObject.layer == gameObject.layer) 
                    return sibling.gameObject.transform;
            }
        }
        return null;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
