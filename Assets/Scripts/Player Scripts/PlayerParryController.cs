using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParryController : MonoBehaviour
{
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float parryRange;
    private bool isParrying;

    private PlayerComponents playerComponents;
    private PlayerAnimationController playerAnimationController;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    public void OnParry(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !PlayerState.isGhost || isParrying) return;

        Collider2D collider = Physics2D.OverlapCircle(playerComponents.ghostTransform.position, parryRange, attackableLayer);
        if (collider == null) return;
        
        if (collider.TryGetComponent(out Parryable parryable))
        {
             playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostParry);
        }
    }
}
