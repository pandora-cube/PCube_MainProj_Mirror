using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearStage : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item item;
    private BoxCollider2D collid;

    private void Start()
    {
        collid = GetComponent<BoxCollider2D>();
    }

    public void Interact()
    {
        if (inventory == null || collid.isTrigger) return;

        if (inventory.FindItem(item))
        {
            inventory.UseItem(item);
            collid.isTrigger = true;

            ProgressData.Instance.playerData.currentStage += 1;
            ProgressData.Instance.SaveData();
            Debug.Log("stage clear");
        }
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}