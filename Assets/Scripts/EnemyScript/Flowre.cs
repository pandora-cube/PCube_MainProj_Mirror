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
    private float attackDelay = 1f;
    private float attackDamage = 1f;

    public float maxHealth { get; set;}
    public float currentHealth { get; set; }

    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        openCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (!playerController.isGhost) CloseWhenPlayerIsNormal();

            
        else if (playerController.isGhost)
        {
            DetectPlayer();

        }
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
        RaycastHit2D circleCast = Physics2D.CircleCast(transform.position, playerDetectionRadius, Vector2.up);

        if (circleCast.collider != null && circleCast.collider.gameObject.CompareTag("Player")) 
        {
            isOpen = true;
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
        //TO-DO: ADD CLOSING ANIM
        isOpen = false;
    }

    public void Die()
    {
        //TO-DO: ADD DEATH ANIM
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
