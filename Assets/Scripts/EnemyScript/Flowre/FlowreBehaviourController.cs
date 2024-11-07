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
        animationController = GetComponent<FlowreAnimationController>();
    }

    protected virtual void Start()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreClosed);
        
        //this needs to be here because if this line is in Awake, it will be called before Animator component is fully initialzied..
        attackDelay = animationController.GetAnimationStateLength(FlowreAnimationController.FlowreAnimationStates.flowreAttack);
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
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreClosed);
    }

    public override IEnumerator TriggerAttackAnimation()
    {
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        animationController.ChangeAnimationState(FlowreAnimationController.FlowreAnimationStates.flowreIdle);
        
        isAttacking = false;
    }    
}
