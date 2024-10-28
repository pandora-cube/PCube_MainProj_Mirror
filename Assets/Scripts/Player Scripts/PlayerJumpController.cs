using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerJumpController : PlayerStateMachine
{
    [Header("Jump Variables")]
    public float jumpForce = 5f;
    bool isHoldingDown = false;

    PlayerGroundChecker playerGroundChecker;

    void Awake()
    {
        playerGroundChecker = GetComponent<PlayerGroundChecker>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!playerGroundChecker.isGrounded || !ctx.performed) return;

        if (isHoldingDown)
        {
            StartCoroutine(JumpDownThroughPlatform());
        }
        else if (playerGroundChecker.isOnSlope)
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
            if (isNormal) normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            else if (isGhost) ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnHoldDown(InputAction.CallbackContext ctx)
    {
        if (ctx.started) isHoldingDown = true;
        else if (ctx.canceled) isHoldingDown = false;
    }

    IEnumerator JumpDownThroughPlatform()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), true);

        if (isNormal) normalRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
        else if (isGhost) ghostRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), false);
    }

}
