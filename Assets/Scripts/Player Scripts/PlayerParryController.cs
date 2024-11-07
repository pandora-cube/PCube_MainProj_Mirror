using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParryController : MonoBehaviour
{
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] float parryRange;

    private PlayerComponents playerComponents;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
    }


    public void OnParry(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !PlayerState.isGhost) return;

        Collider2D collider = Physics2D.OverlapCircle(playerComponents.ghostTransform.position, parryRange, attackableLayer);
        if (collider == null) return;
        if (collider.TryGetComponent(out Parryable parryable))
        {
            parryable.AttemptParry();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawWireSphere(playerComponents.ghostTransform.position, parryRange);
    }
}
