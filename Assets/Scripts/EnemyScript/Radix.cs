using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Radix : MonoBehaviour, IDamageable
{
    [SerializeField] private Fleaore[] connectedFleaore;
    private BoxCollider2D boxCollider2D;
    private Animator radixAnimator;
    private PlayerController playerController;

    [field: SerializeField] public float maxHealth { get; set ; }
    [field: SerializeField] public float currentHealth { get; set; }

    [SerializeField] private bool isEmerged = false;
    [SerializeField] private float speed = 2f;

    private string currentState;
    enum RadixAnimationStates
    {
        radixIdle,
        radixEmerge,
        radixMove,
        radixAttack
    }
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        radixAnimator = GetComponent<Animator>();
        boxCollider2D.enabled = false;
        playerController = FindObjectOfType<PlayerController>();

        maxHealth = 5;
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (!isEmerged && IsFleaoresAllDestroyed())
        {
            Emerge();
        }

        if (isEmerged) MoveTowardsPlayer();
    }
    private bool IsFleaoresAllDestroyed()
    {
        foreach (Fleaore fleaore in connectedFleaore)
            if (fleaore != null && fleaore.gameObject != null) return false;
        
        return true;
    }
    private void Emerge()
    {
        isEmerged = true;
        boxCollider2D.enabled = true;
        //
    }
    private void MoveTowardsPlayer()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(0f, 0f);
        if (playerController.isNormal) targetPosition = playerController.normalGameObejct.transform.position;
        else targetPosition = playerController.ghostGameObejct.transform.position;

        float direction = Mathf.Sign(targetPosition.x - currentPosition.x);

        Vector3 newPosition = new Vector3(currentPosition.x + direction * speed * Time.deltaTime, currentPosition.y, currentPosition.z);
        transform.position = newPosition;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void ChangeAnimationState (RadixAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;
        
        radixAnimator.Play(newState);
        currentState = newState;
    }

}
