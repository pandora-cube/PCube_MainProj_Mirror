using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float normalInteractRange = 0.5f;
    [SerializeField] private float ghostInteractRange = 1f;

    [SerializeField] private LayerMask interactableLayer;

    private PlayerComponents playerComponents;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    private Inventory inventory;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
        inventory = GetComponent<Inventory>();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            IInteractable interactable = GetInteractableObject();

            if (interactable != null) interactable.Interact();
        }
    }

    /// <summary>
    /// function for finding the nearest interactable object
    /// </summary>
    /// <returns>nearest interactable object found</returns>
    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider2D collider = null;
        if (PlayerState.isNormal)
        {
            collider = Physics2D.OverlapCircle(playerComponents.normalTransform.position, normalInteractRange, interactableLayer);
        }
        else if (PlayerState.isGhost)
        {
            collider = Physics2D.OverlapCircle(playerComponents.ghostTransform.position, ghostInteractRange, interactableLayer);
        }

        if (collider == null) return null;

        if (collider.gameObject.CompareTag("Player")) return null;

        if (collider.TryGetComponent(out IInteractable interactable))
        {
            interactableList.Add(interactable);
        }


        IInteractable closestInteractable = null;
        foreach (IInteractable I in interactableList)
        {
            if (closestInteractable == null) closestInteractable = I;
            else
            {
                if (Vector2.Distance(playerComponents.normalTransform.position, I.GetTransform().position) <
                    Vector2.Distance(playerComponents.normalTransform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = I;
                }
            }
        }

        return closestInteractable;
    }
    public void Transform()
    {
        PlayerState.isNormal = !PlayerState.isNormal;
        PlayerState.isGhost = !PlayerState.isGhost;

        playerComponents.normalGameObject.SetActive(PlayerState.isNormal); playerComponents.ghostGameObject.SetActive(PlayerState.isGhost);
        if (PlayerState.isGhost) inventory.ClearInventory();
    }
}
