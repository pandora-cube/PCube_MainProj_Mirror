using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_PlayerMovementController : PlayerController
{
    #region dash variables
    private bool isGhost = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    [SerializeField] TrailRenderer trailRenderer;

    #endregion

    void FixedUpdate()
    {
        if (!isDashing) 
            base.FixedUpdate();
        
    }

    void Teleport()
    {

    }
    private IEnumerator Dash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;

            //대쉬 중 떨어지지 않게 gravity 값 변경
            float originalGravity = normal_rb.gravityScale;
            normal_rb.gravityScale = 0f;

            normal_rb.velocity = new Vector2(direction * dashForce, 0f);
            trailRenderer.emitting = true;

            yield return new WaitForSeconds(dashTime);

            trailRenderer.emitting = false;
            normal_rb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
    void Smoke()
    {

    }
}
