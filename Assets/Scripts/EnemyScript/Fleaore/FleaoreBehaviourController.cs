using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaoreBehaviourController : EnemyBehaviour
{
    private FleaoreAnimationController animationController;
    private Vine connectedVine;
    private Damageable damageable;

    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    private bool isAttacked = false;
    private bool isStunned = false;

    void Awake()
    {
        base.Awake();
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

    override public IEnumerator TriggerAttackAnimation()
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
        boxCollider2D.enabled = false;
    }
}
