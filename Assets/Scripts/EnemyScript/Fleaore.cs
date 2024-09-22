using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fleaore : MonoBehaviour, IDamageable
{
    [SerializeField] private Vine connectedVine;

    private PlayerController playerController;
    private BoxCollider2D openCollider2D;
    private Animator fleaoreAnimator;

    private bool isAttacking = false;
    private bool isAttacked = false;
    [SerializeField] private bool isStunned = false;
    private float playerDetectionRadius = 5f;
    [SerializeField] private float attackDelay = 1f;
    private float attackDamage = 1f;
    const int PLAYER_LAYER = 3;

    private string currentState;
    enum FleaoreAnimationStates
    {
        fleaoreIdle,
        fleaoreAttack
    }

    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        openCollider2D = GetComponent<BoxCollider2D>();
        fleaoreAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isStunned) return;

        if (!playerController.isGhost || isAttacked) Close();
        else if (playerController.isGhost) DetectPlayer();
    }

    void Close()
    {
        openCollider2D.enabled = false;
    }

    void DetectPlayer()
    {
        //TO-DO: ADD ANIM TRIGGER FOR OPENING
        openCollider2D.enabled = true; //also allows Floaore to be attackable.
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
        ChangeAnimationState(FleaoreAnimationStates.fleaoreAttack);
        yield return new WaitForSeconds(attackDelay);
        ChangeAnimationState(FleaoreAnimationStates.fleaoreIdle);
        
        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Fleaore takes damage");
        //TO-DO: ADD CLOSING ANIM AND DMG TAKE LOGIC
        isAttacked = true;
        StartCoroutine(Reopen());
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (connectedVine == null) Die();
            else StartCoroutine(Revive());
        }

    }
    
    IEnumerator Reopen()
    {
        yield return new WaitForSeconds(5f);
        isAttacked = false;
    }
    public void Die()
    {
        //TO-DO: ADD DEATH ANIM
        Debug.Log("Fleaore dies");
        Destroy(gameObject);
    }

    IEnumerator Revive()
    {
        isStunned = true;
        yield return new WaitForSeconds(5);
        if (connectedVine == null)
        {
            Die();
            yield break;
        }

        isStunned = false;
        currentHealth = maxHealth;
        //TODO: Add anim for revival
    }

    private void ChangeAnimationState(FleaoreAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        fleaoreAnimator.Play(newState);
        currentState = newState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
