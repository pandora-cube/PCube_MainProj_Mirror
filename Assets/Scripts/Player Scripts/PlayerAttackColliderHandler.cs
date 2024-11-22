using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackColliderHandler : MonoBehaviour
{
    private PlayerAttackManager playerAttackManager;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerAttackManager = GetComponentInParent<PlayerAttackManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        Parryable parryable = collider2D.GetComponentInParent<Parryable>();
        if (parryable != null && parryable.IsInvincible()) return;

        if (collider2D.gameObject.CompareTag("AttackCollider"))
        {
            playerAttackManager.HandleAttackCollision(collider2D);
        }
        else if (collider2D.gameObject.CompareTag("Enemy"))
        {
            playerAttackManager.HandleAttackCollision(collider2D);
        }
    }
}
