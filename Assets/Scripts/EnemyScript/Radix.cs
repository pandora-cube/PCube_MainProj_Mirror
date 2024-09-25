using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Radix : MonoBehaviour
{
    [SerializeField] private Fleaore[] connectedFleaore;
    private BoxCollider2D boxCollider2D;
    private Animator radixAnimator;
    private PlayerController playerController;

    [SerializeField] private bool isEmerged = false;
    [SerializeField] private float speed = 2f;

    [SerializeField]private float playerDetectionRadius = 5f;
    private bool isAttacking = false;
    private float attackDamage = 1f;
    const int PLAYER_LAYER = 3;
    [SerializeField] private float attackDelay;
    [SerializeField] private float emergeDelay;

    private string currentState;
    enum RadixAnimationStates
    {
        radixIdle,
        radixEmerged,
        radixMove,
        radixAttack
    }
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        radixAnimator = GetComponent<Animator>();
        boxCollider2D.enabled = false;
        playerController = FindObjectOfType<PlayerController>();

        ChangeAnimationState(RadixAnimationStates.radixIdle);

    }
    void Update()
    {
        if (!isEmerged && IsFleaoresAllDestroyed())
        {
            StartCoroutine(Emerge());
        }

        if (isEmerged)
        {
            if (!isAttacking) MoveTowardsPlayer();
            DetectPlayer();
        }
    }
    private bool IsFleaoresAllDestroyed()
    {
        foreach (Fleaore fleaore in connectedFleaore)
            if (fleaore != null && fleaore.gameObject != null) return false;

        return true;
    }
    private IEnumerator Emerge()
    {
        yield return new WaitForSeconds(emergeDelay);
        isEmerged = true;
        boxCollider2D.enabled = true;
    }
    private void MoveTowardsPlayer()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(0f, 0f);
        if (playerController.isNormal) targetPosition = playerController.normalGameObject.transform.position;
        else targetPosition = playerController.ghostGameObject.transform.position;

        float direction = Mathf.Sign(targetPosition.x - currentPosition.x);
        if (Mathf.Approximately(direction, 1)) 
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
        }
        Vector3 newPosition = new Vector3(currentPosition.x + direction * speed * Time.deltaTime, currentPosition.y, currentPosition.z);
        transform.position = newPosition;

        ChangeAnimationState(RadixAnimationStates.radixMove);
    }

    void DetectPlayer()
    {
        boxCollider2D.enabled = true;

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

        ChangeAnimationState(RadixAnimationStates.radixAttack);
        float animLength = GetAnimationStateLength(RadixAnimationStates.radixAttack.ToString()); // get attack anim length;
        Debug.Log(animLength);
        yield return new WaitForSeconds(animLength);

        ChangeAnimationState(RadixAnimationStates.radixEmerged); //go back to emerged once attack anim is finished
        yield return new WaitForSeconds(attackDelay - animLength); //wait until end of attack delay;
        isAttacking = false;
    }

    private void ChangeAnimationState(RadixAnimationStates animationStates)
    {
        string newState = animationStates.ToString();
        if (currentState == newState) return;

        radixAnimator.Play(newState);
        currentState = newState;
    }

    /// <summary>
    /// finds the given animation state's length
    /// </summary>
    /// <param name="stateName">name of the animation state in the animation controller</param>
    /// <returns>length of animation state in float</returns>
    float GetAnimationStateLength(string stateName)
    {
        if (radixAnimator == null || radixAnimator.runtimeAnimatorController == null)
            return 0f;

        RuntimeAnimatorController controller = radixAnimator.runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == stateName)
            {
                return clip.length;
            }
        }

        return 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
