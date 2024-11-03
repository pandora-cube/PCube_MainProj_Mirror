using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowreBehaviourController : EnemyBehaviour
{
    private FlowreAnimationController animationController;
    public PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    
    void Awake()
    {
        base.Awake();
        animationController = GetComponent<FlowreAnimationController>();
        boxCollider2D.enabled = false;
    }

    protected virtual void Start()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreClosed);
    }

    protected virtual void Update()
    {
        if (PlayerState.isNormal) 
        {
            Close();  
        }
        else if (PlayerState.isGhost) 
        {
            DetectPlayer();
        }
    }

    protected void Close()
    {
        boxCollider2D.enabled = false;
    }

    public override IEnumerator TriggerAttackAnimation()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreIdle);
        isAttacking = false;
    }
}
