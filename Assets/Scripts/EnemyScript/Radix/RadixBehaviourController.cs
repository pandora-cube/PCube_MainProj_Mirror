using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadixBehaviour : EnemyBehaviour
{
    private RadixAnimationController animator;
    private PlayerComponents playerComponents;

    private bool isEmerged = false;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float emergeDelay;

    [SerializeField] private FleaoreBehaviourController[] connectedFleaore;

    void Awake()
    {
        base.Awake();
    }
    
    void Start()
    {
        playerComponents = FindObjectOfType<PlayerComponents>();
        animator = GetComponentInChildren<RadixAnimationController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
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
        foreach (FleaoreBehaviourController fleaore in connectedFleaore)
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
        Vector3 targetPosition = DetermineTargetPosition();
        
        float direction = Mathf.Sign(targetPosition.x - currentPosition.x);
        FlipSpriteBasedOnDirection(direction);

        Vector3 newPosition = new Vector3(currentPosition.x + direction * speed * Time.deltaTime, currentPosition.y, currentPosition.z);
        transform.position = newPosition;

        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixMove);
    }

    private Vector3 DetermineTargetPosition()
    {
        if (PlayerStateMachine.instance.isNormal) return playerComponents.normalGameObject.transform.position;
        else return playerComponents.ghostGameObject.transform.position;
    }
    private void FlipSpriteBasedOnDirection(float direction)
    {
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
    }

    override public IEnumerator TriggerAttackAnimation()
    {
        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixAttack);
        float animLength = animator.GetAnimationStateLength(RadixAnimationController.RadixAnimationStates.radixAttack.ToString()); // get attack anim length;
        yield return new WaitForSeconds(animLength);

        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixEmerged); //go back to emerged once attack anim is finished
        yield return new WaitForSeconds(attackDelay - animLength); //wait until end of attack delay;
    }
}
