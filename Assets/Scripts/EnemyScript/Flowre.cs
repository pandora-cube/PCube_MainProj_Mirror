using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flowre : Damageable
{
    private CapsuleCollider2D openCapsuleCollider2D;

    private bool isAttacking = false;
    [SerializeField] private float playerDetectionRadius = 5f;
    [SerializeField] private float attackDelay = 1f;
    protected float attackDamage = 1f;

    private Animator flowreAnimator;

    private string currentState;
    enum FlowreAnimationStates
    {
        flowreClosed,
        flowreIdle,
        flowreAttack
    }

    void Awake()
    {
        openCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        flowreAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        ChangeAnimationState(FlowreAnimationStates.flowreClosed);
        currentHealth = maxHealth;
    }
    protected virtual void Update()
    {
        if (!PlayerStateMachine.instance.isGhost) Close();  
        else if (PlayerStateMachine.instance.isGhost) DetectPlayer();
    }

    protected void Close()
    {
        openCapsuleCollider2D.enabled = false;
    }

    public override void DetectPlayer()
    {
        openCapsuleCollider2D.enabled = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius, 1 << PLAYER_LAYER);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider.CompareTag("Player"))
            {
                if (isAttacking) return;

                StartCoroutine(AttackPlayer(hitCollider));
            }
        }
    }
    
    public override IEnumerator AttackPlayer(Collider2D playerCollider)
    {
        isAttacking = true;
        IDamageable player = playerCollider.gameObject.GetComponent<IDamageable>();
        player.TakeDamage(attackDamage);
        ChangeAnimationState(FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        ChangeAnimationState(FlowreAnimationStates.flowreIdle);
        isAttacking = false;
    }

    public override void TakeDamage(int damageAmount)
    {
        //TO-DO: ADD CLOSING ANIM
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
    }

    public override void Die()
    {
        //TO-DO: ADD DEATH ANIM
        Destroy(gameObject);
    }

     private void ChangeAnimationState (FlowreAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;
        
        flowreAnimator.Play(newState);
        currentState = newState;
    }

    float GetAnimationStateLength(string stateName)
    {
        if (flowreAnimator == null) return 0f;

        for (int i = 0; i < flowreAnimator.layerCount; i++)
        {
            AnimatorStateInfo stateInfo = flowreAnimator.GetCurrentAnimatorStateInfo(i);

            if (stateInfo.IsName(stateName)) return stateInfo.length;
        }
        return 0f;
    }
}
