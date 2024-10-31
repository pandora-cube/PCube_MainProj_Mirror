using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected BoxCollider2D boxCollider2D;

    public const int PLAYER_LAYER = 3;

    [SerializeField] protected float playerDetectionRadius;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackDelay;

    protected bool isAttacking;
    
    protected void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected void DetectPlayer()
    {
        if (isAttacking) return;

        boxCollider2D.enabled = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius, 1 << PLAYER_LAYER);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider == null || hitCollider.CompareTag("Player")) continue;

            AttackPlayer(hitCollider);
        }
    }

    protected void AttackPlayer(Collider2D playerCollider)
    {
        isAttacking = true;
        Damageable player = playerCollider.gameObject.GetComponent<Damageable>();
        player.TakeDamage(attackDamage);
        StartCoroutine(TriggerAttackAnimation());

        isAttacking = false;
    }

    public abstract IEnumerator TriggerAttackAnimation();
}
