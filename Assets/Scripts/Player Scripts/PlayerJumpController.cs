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
    PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
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
            if (PlayerState.isNormal) playerComponents.normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            else if (PlayerState.isGhost) playerComponents.ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnHoldDown(InputValue value)
    {
        isHoldingDown = value.isPressed;
    }

    IEnumerator JumpDownThroughPlatform()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), true);

        if (PlayerState.isNormal) playerComponents.normalRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
        else if (PlayerState.isGhost) playerComponents.ghostRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), false);
    }

}
