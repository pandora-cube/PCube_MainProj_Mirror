using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackManager : MonoBehaviour
{
    const int ATTACKABLE_LAYER = 12;

    #region ATTACK VARIABLES
    [Header("Attack Variables")]
    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private LayerMask attackableLayer;

    [SerializeField] private float comboResetTime = 1f; // Time allowed between combo attacks
    [SerializeField] private float lastAttackTime;
    [SerializeField] private int comboAttackNumber = 0;
    #endregion

    private PlayerAnimationController playerAnimationController;
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    void Update()
    {
        if (PlayerState.isGhost && (Time.time - lastAttackTime > comboResetTime))
        {
            comboAttackNumber = 0;
            PlayerState.isAttacking = false;
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!PlayerState.isGhost || !PlayerState.isGrounded || !context.performed) return;
        PlayerState.isAttacking = true;
        lastAttackTime = Time.time;

        comboAttackNumber++;

        if (comboAttackNumber > 3) comboAttackNumber = 1;

        if (playerAnimationController.CheckIfAttackAnimationHasEnded()) TriggerAttackAnimation();
        else comboAttackNumber--;
    }


    private void TriggerAttackAnimation()
    {
        switch (comboAttackNumber)
        {
            case 1: playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostAttack1); break;
            case 2: playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostAttack2); break;
            case 3: playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostAttack3); break;
        }
    }


    public void HandleAttackCollision(Collider2D collision)
    {
        //Parryable parryable = collision.gameObject.GetComponent<Parryable();
        //if (parryable != null)
        //{
            //parryable.Parry()
            //return;
        //}

        if (collision.gameObject.CompareTag("Player")) return;

        Debug.Log(collision.gameObject.name);
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        Knockbackable knockbackable = collision.gameObject.GetComponent<Knockbackable>();
        if (damageable == null) return;

        damageable.TakeDamage(1);

        if (knockbackable == null) return;
        knockbackable.ApplyKnockback(gameObject.transform);
    }
}
