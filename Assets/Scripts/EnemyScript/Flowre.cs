using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flowre : MonoBehaviour, IDamageable
{
    private PlayerController playerController;
    private CapsuleCollider2D openCapsuleCollider2D;

    [SerializeField] private bool isOpen = false;
    private bool isAttacking = false;
    private float playerDetectionRadius = 5f;
    [SerializeField] private float attackDelay = 1f;
    private float attackDamage = 1f;

    [field: SerializeField] public float maxHealth { get; set;}
    [field: SerializeField] public float currentHealth { get; set; }

    [SerializeField] private Animator flowreAnimator;
    private AnimatorStateInfo currentAnimation;
    private string currentState;
    enum FlowreAnimationStates
    {
        flowreIdle,
        flowreAttack
    }

    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        openCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (!playerController.isGhost) CloseWhenPlayerIsNormal();  
        else if (playerController.isGhost) DetectPlayer();
    }
    
    void CloseWhenPlayerIsNormal()
    {
        openCapsuleCollider2D.enabled = false;
        isOpen = false;
    }

    void DetectPlayer()
    {
        //TO-DO: ADD ANIM TRIGGER FOR OPENING
        openCapsuleCollider2D.enabled = true; //also allows Flowre to be attackable.
        Collider2D circleCast = Physics2D.OverlapCircle(transform.position, playerDetectionRadius);

        if (circleCast != null && circleCast.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Circle cast: " + circleCast.gameObject.name);
            isOpen = true;
            Debug.Log("TOUCH");
            if (isAttacking) return;
            StartCoroutine(AttackPlayer(circleCast));
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

    public void TakeDamage(float damageAmount)
    {
        //TO-DO: ADD CLOSING ANIM
        isOpen = false;
        currentHealth -= damageAmount;
    }

    public void Die()
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
