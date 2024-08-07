using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowre : MonoBehaviour, IDamageable
{
    private PlayerController playerController;
    private CapsuleCollider2D openCapsuleCollider2D;

    [SerializeField] private bool isOpen = false;
    private float playerDetectionRadius = 1f;
    private float attackDelay = 1f;

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

            if (isOpen) StartCoroutine(AttackPlayer());
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

        if (circleCast.collider.gameObject.CompareTag("Player")) isOpen = true;
    }

    IEnumerator AttackPlayer()
    {
        //TO-DO: ADD ANIM TRIGGER FOR ATTACK
        yield return new WaitForSeconds(attackDelay);
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
}
