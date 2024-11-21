using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public void HandleAttackCollision(Collider2D collider2D)
    {        
        PlayerGhostHealthManager playerGhostHealthManager = collider2D.gameObject.GetComponent<PlayerGhostHealthManager>();
        Knockbackable knockbackable = collider2D.gameObject.GetComponent<Knockbackable>();

        if (knockbackable == null) return;

        playerGhostHealthManager.TakeDamage(1);
        knockbackable.ApplyKnockback(gameObject.transform);
    }
}
