using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    public static PlayerInteractionController instance;
    [SerializeField] private float normalInteractRange = 0.5f;
    [SerializeField] private float ghostInteractRange = 1f;

    [SerializeField] private LayerMask interactableLayer;

    private PlayerComponents playerComponents;
    private PlayerHorizontalMovement horizontalMovement;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    private Inventory inventory => Inventory.instance;

    void Awake()
    {
        #region singleton
        if (instance == null) instance = this;
        else Destroy(instance);
        #endregion

        playerComponents = GetComponent<PlayerComponents>();
        horizontalMovement = GetComponent<PlayerHorizontalMovement>();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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

        if (!PlayerCameraController.instance.isProducting)
            PlayerCameraController.instance.ReturnCameraPosition();

        playerComponents.normalGameObject.SetActive(PlayerState.isNormal); playerComponents.ghostGameObject.SetActive(PlayerState.isGhost);
        if (PlayerState.isGhost) inventory.ClearInventory();

        horizontalMovement.UpdateOtherTransformObjectPosition();

        PlayerState.isAttacking = PlayerState.isGhost;
    }
}
