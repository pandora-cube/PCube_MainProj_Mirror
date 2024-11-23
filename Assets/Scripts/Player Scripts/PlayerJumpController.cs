using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump Variables")]
    public float jumpForce = 5f;
    bool isHoldingDown = false;

    PlayerComponents playerComponents;
    PlayerAnimationController playerAnimationController;  
    PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        PlayerState.OnLanded.AddListener(TriggerLandingAnimation);
    }

    private void OnDestroy()
    {
        PlayerState.OnLanded.RemoveAllListeners();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        float input = value.ReadValue<float>();
        if (!PlayerState.isGrounded || input <= 0) return;

        if (isHoldingDown)
        {
            StartCoroutine(JumpDownThroughPlatform());
        }
        else if (PlayerState.isOnSlope)
        {
            // slope일때 점프 수정 필요

            //Debug.Log("slopeJump");
            //Vector2 jumpDirection = (slopeNormalPrep + Vector2.up).normalized;
            //if (isNormal)
            //{
            //    normalRb.sharedMaterial = noFiction;
            //    normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            //}
            //else if (isGhost)
            //{
            //    ghostRb.sharedMaterial = noFiction;
            //    ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            //}
        }
        else
        {
            if (PlayerState.isNormal)
            {
                playerComponents.normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalJumpStart);
            }
            else if (PlayerState.isGhost) playerComponents.ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnHoldDown(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            isHoldingDown = true;
        }
        else if (context.canceled)
        {
            isHoldingDown = false;
        }
    }

    IEnumerator JumpDownThroughPlatform()
    {
        if (PlayerState.isNormal)
        {
            Physics2D.IgnoreLayerCollision(playerComponents.normalRb.gameObject.layer, LayerMask.NameToLayer("Bridge"), true);
            playerComponents.normalRb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalJumpStart);
        }
        else if (PlayerState.isGhost)
        {
            Physics2D.IgnoreLayerCollision(playerComponents.ghostRb.gameObject.layer, LayerMask.NameToLayer("Bridge"), true);
            playerComponents.ghostRb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1.5f);

        Physics2D.IgnoreLayerCollision(playerComponents.normalRb.gameObject.layer, LayerMask.NameToLayer("Bridge"), false);
        Physics2D.IgnoreLayerCollision(playerComponents.ghostRb.gameObject.layer, LayerMask.NameToLayer("Bridge"), false);
    }

    private void TriggerLandingAnimation()
    {
        if (PlayerState.isNormal)
        {
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalJumpEnd);
        }
    }
}
