using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected BoxCollider2D boxCollider2D;

    [SerializeField] protected float playerDetectionRadius;
    [SerializeField] protected float attackDamage;
    protected float attackDelay;

    [SerializeField] protected bool isAttacking;

    private Coroutine attackCoroutine;

    protected void DetectPlayer()
    {
        if (isAttacking) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider == null || !hitCollider.CompareTag("Player")) continue;

            AttackPlayer(hitCollider);
        }
    }

    protected void AttackPlayer(Collider2D playerCollider)
    {
        isAttacking = true;
        PlayerGhostHealthManager player = playerCollider.gameObject.GetComponent<PlayerGhostHealthManager>();
        player.TakeDamage(attackDamage);

        StartCoroutine(TriggerAttackAnimation());
    }

    public abstract IEnumerator TriggerAttackAnimation();
}
