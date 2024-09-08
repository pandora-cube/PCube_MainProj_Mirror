using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleaore : MonoBehaviour
{
    [SerializeField] private Vine connectedVine;

    private PlayerController playerController;
    private BoxCollider2D openCollider2D;

    [SerializeField] private bool isOpen = false;
    private bool isAttacking = false;
    [SerializeField] private bool isStunned = false;
    private float playerDetectionRadius = 5f;
    private float attackDelay = 1f;
    private float attackDamage = 1f;

    [field:SerializeField] public float maxHealth { get; set;}
    [field:SerializeField] public float currentHealth { get; set; }

    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        openCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isStunned) return;

        if (!playerController.isGhost) CloseWhenPlayerIsNormal();
        else if (playerController.isGhost) DetectPlayer();
    }
    
    void CloseWhenPlayerIsNormal()
    {
        openCollider2D.enabled = false;
        isOpen = false;
    }

    void DetectPlayer()
    {
        //TO-DO: ADD ANIM TRIGGER FOR OPENING
        openCollider2D.enabled = true; //also allows Floaore to be attackable.
        RaycastHit2D circleCast = Physics2D.CircleCast(transform.position, playerDetectionRadius, Vector2.up);

        if (circleCast.collider != null && circleCast.collider.gameObject.CompareTag("Player")) 
        {
            isOpen = true;
            Debug.Log("TOUCH");
            if (isAttacking) return;
            StartCoroutine(AttackPlayer(circleCast.collider));
        }
    }

    IEnumerator AttackPlayer(Collider2D collider)
    {
        isAttacking = true;
        IDamageable player = collider.gameObject.GetComponent<IDamageable>();
        player.TakeDamage(attackDamage);
        //TO-DO: ADD ANIM TRIGGER FOR ATTACK
        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        //TO-DO: ADD CLOSING ANIM AND DMG TAKE LOGIC
        isOpen = false;
        currentHealth -=damageAmount;

        if (currentHealth <= 0)
        {
            if (connectedVine == null) Die();
            else StartCoroutine(Revive());
        }

    }

    public void Die()
    {
        //TO-DO: ADD DEATH ANIM
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
        //TODO: Add anim for revival
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
