using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flowre : MonoBehaviour
{
    protected PlayerController playerController;
    private CapsuleCollider2D openCapsuleCollider2D;

    private bool isAttacking = false;

    [SerializeField] private float playerDetectionRadius = 5f;
    [SerializeField] private float attackDelay = 1f;
    protected float attackDamage = 1f;

    private Animator flowreAnimator;

    const int PLAYER_LAYER = 3;
    private string currentState;
    enum FlowreAnimationStates
    {
        flowreClosed,
        flowreIdle,
        flowreAttack
    }

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        openCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        flowreAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        ChangeAnimationState(FlowreAnimationStates.flowreClosed);
    }
    protected virtual void Update()
    {
        if (!playerController.isGhost) Close();  
        else if (playerController.isGhost) DetectPlayer();
    }

    protected void Close()
    {
        openCapsuleCollider2D.enabled = false;
    }

    void DetectPlayer()
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
    
    IEnumerator AttackPlayer(Collider2D collider)
    {
        isAttacking = true;
        IDamageable player = collider.gameObject.GetComponent<IDamageable>();
        player.TakeDamage(attackDamage);
        ChangeAnimationState(FlowreAnimationStates.flowreAttack);
        yield return new WaitForSeconds(attackDelay);
        ChangeAnimationState(FlowreAnimationStates.flowreIdle);
        isAttacking = false;
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
    private void OnDrawGizmos()
{
    // Visualize detection radius as a wire sphere at the object's position
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

    // Visualize the direction of the CircleCast (upwards in your case)
    Vector3 castDirection = Vector3.up; // You can change this to any direction you're casting
    float castDistance = 1f; // This is the distance of your CircleCast, adjust as needed

    // Draw a line to show the cast direction
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + castDirection * castDistance);

    // Draw a wire sphere at the end of the cast to show where the circle would be
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position + castDirection * castDistance, playerDetectionRadius);
}
}
