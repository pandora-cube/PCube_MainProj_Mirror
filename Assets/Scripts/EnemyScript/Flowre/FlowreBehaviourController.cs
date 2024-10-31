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

    override public IEnumerator TriggerAttackAnimation()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreIdle);
        isAttacking = false;
    }
}
