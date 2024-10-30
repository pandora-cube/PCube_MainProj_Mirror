using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaoreBehaviourController : EnemyBehaviour
{
    private BoxCollider2D openCollider2D;
    private FleaoreAnimationController animationController;
    private Vine connectedVine;
    private Damageable damageable;

    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    private bool isAttacking;
    private bool isAttacked = false;
    private bool isStunned = false;

    void Awake()
    {
        openCollider2D = GetComponent<BoxCollider2D>();
        animationController = GetComponent<FleaoreAnimationController>();
        connectedVine = GetComponentInChildren<Vine>();
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (isStunned) return;

        if (PlayerState.isNormal || isAttacked) Close();
        DetectPlayer();
    }


    public override void DetectPlayer()
    {
        if (isAttacking) return;

        //TO-DO: ADD ANIM TRIGGER FOR OPENING
        openCollider2D.enabled = true; //also allows Floaore to be attackable.

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius, 1 << PLAYER_LAYER);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider == null || hitCollider.CompareTag("Player")) continue;
    
            AttackPlayer(hitCollider);
        }
    }

    public override void AttackPlayer(Collider2D collider)
    {
        isAttacking = true;
        Damageable player = collider.gameObject.GetComponent<Damageable>();
        player.TakeDamage(attackDamage);
        StartCoroutine(TriggerAttackAnimation());

        isAttacking = false;
    }

    public IEnumerator TriggerAttackAnimation()
    {
        animationController.ChangeAnimationState(FleaoreAnimationController.FleaoreAnimationStates.fleaoreAttack);
        yield return new WaitForSeconds(attackDelay);
        animationController.ChangeAnimationState(FleaoreAnimationController.FleaoreAnimationStates.fleaoreIdle);
        isAttacking = false;
    }

    IEnumerator Reopen()
    {
        yield return new WaitForSeconds(5f);
        isAttacked = false;
    }

    IEnumerator Revive()
    {
        isStunned = true;
        yield return new WaitForSeconds(5);
        if (connectedVine == null)
        {
            damageable.Die();
            yield break;
        }

        isStunned = false;
        damageable.currentHealth = damageable.maxHealth;
        //TODO: Add anim for revival
    }


    void Close()
    {
        openCollider2D.enabled = false;
    }
}
