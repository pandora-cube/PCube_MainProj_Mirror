using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : PlayerStateMachine
{
    [SerializeField] private float normalInteractRange = 0.5f;
    [SerializeField] private float ghostInteractRange = 1f;

    [SerializeField] private LayerMask interactableLayer;


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
        if (isNormal)
        {
            collider = Physics2D.OverlapCircle(normalTransform.position, normalInteractRange, interactableLayer);
        }
        else if (isGhost)
        {
            collider = Physics2D.OverlapCircle(ghostTransform.position, ghostInteractRange, interactableLayer);
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
                if (Vector2.Distance(normalTransform.position, I.GetTransform().position) <
                    Vector2.Distance(normalTransform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = I;
                }
            }
        }

        return closestInteractable;
    }
    public void Transform()
    {
        isNormal = !isNormal;
        isGhost = !isGhost;

        normalGameObject.SetActive(isNormal); ghostGameObject.SetActive(isGhost);
        if (isGhost) inventory.ClearInventory();
    }
}
