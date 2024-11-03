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
            if (playerAnimationController == null) Debug.Log("NULL!!!");
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
        }
    }


    public void OnAttack(InputValue value)
    {
        float input = value.Get<float>();
        if (!PlayerState.isGhost || input == 0f) return;

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
        if (collision.gameObject.layer == ATTACKABLE_LAYER)
        {
            Damageable damageable = collision.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(1);
            }
        }
    }
}
