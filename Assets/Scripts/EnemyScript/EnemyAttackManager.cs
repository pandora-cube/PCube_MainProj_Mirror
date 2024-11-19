using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public void HandleAttackCollision(Collider2D collider2D)
    {
        PlayerGhostHealthManager playerGhostHealthManager = collider2D.gameObject.GetComponent<PlayerGhostHealthManager>();
        Damageable damageable = collider2D.gameObject.GetComponent<Damageable>();

        if (damageable == null) return;

        playerGhostHealthManager.TakeDamage(1);
        damageable.ApplyKnockback(gameObject.transform);
    }
}
