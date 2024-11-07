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

    
    void Start()
    {
        playerComponents = FindObjectOfType<PlayerComponents>();
        animator = GetComponentInChildren<RadixAnimationController>();

        attackDelay = animator.GetAnimationStateLength(RadixAnimationController.RadixAnimationStates.radixAttack);
        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixIdle);
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
        DetectPlayer();
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

    public override IEnumerator TriggerAttackAnimation()
    {
        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixAttack);
        yield return new WaitForSeconds(attackDelay);

        animator.ChangeAnimationState(RadixAnimationController.RadixAnimationStates.radixEmerged); //go back to emerged once attack anim is finished
        
        isAttacking = false;
    }
}
