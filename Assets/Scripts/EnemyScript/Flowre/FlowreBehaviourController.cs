using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowreBehaviourController : EnemyBehaviour
{
    private FlowreAnimationController animationController;
    private CapsuleCollider2D openCapsuleCollider2D;
    public PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    private bool isAttacking = false;
    
    void Awake()
    {
        animationController = GetComponent<FlowreAnimationController>();
        openCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreClosed);
    }

    protected virtual void Update()
    {
        if (PlayerState.isNormal) Close();  
        else if (PlayerState.isGhost) DetectPlayer();
    }

    protected void Close()
    {
        openCapsuleCollider2D.enabled = false;
    }

    public override void DetectPlayer()
    {
        if (isAttacking) return;

        openCapsuleCollider2D.enabled = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius, 1 << PLAYER_LAYER);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider == null || !hitCollider.CompareTag("Player")) continue;
        
            AttackPlayer(hitCollider);
        }
    }
    
    public override void AttackPlayer(Collider2D playerCollider)
    {
        isAttacking = true;
        Damageable player = playerCollider.gameObject.GetComponent<Damageable>();
        player.TakeDamage(attackDamage);

        StartCoroutine(TriggerAttackAnimation());
        isAttacking = true;
    }

     public IEnumerator TriggerAttackAnimation()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreIdle);
        isAttacking = false;
    }
}
