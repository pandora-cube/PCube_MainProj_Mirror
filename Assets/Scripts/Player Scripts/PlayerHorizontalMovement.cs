using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontalMovement : PlayerStateMachine
{
    #region MOVEMENT VARIABLES
    [SerializeField]  private float direction = 0;
    [SerializeField]  private float normalSpeed;
    [SerializeField]  private float ghostSpeed;
    [HideInInspector] public PolygonCollider2D currentConfinerCollider;
    #endregion

    #region DASH VARIABLES
    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    [SerializeField] TrailRenderer trailRenderer;
    #endregion

    PlayerAnimationController playerAnimationController;
    PlayerGroundChecker playerGroundChecker;
    
    void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerGroundChecker = GetComponent<PlayerGroundChecker>();
    }

    void Update()
    {
        if (!canMove)
        {
            direction = 0f;
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalIdle);
        }
    }


    void FixedUpdate()
    {
        if (isDashing) return;

         if (isNormal && !isGhost)
        {
            MovePlayer(normalRb, normalTransform);
            ConfinePlayerMovement();
        }
        else if (isGhost && isNormal)
        {
            MovePlayer(ghostRb, ghostTransform);
            ConfinePlayerMovement();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (isDashing) return;

        direction = ctx.ReadValue<float>();

        if (isNormal)
        {
            if (Mathf.Approximately(direction, 0f)) playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalIdle);
            else playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalWalk);
        }
        else if (isGhost)
        {
            if (Mathf.Approximately(direction, 0f)) playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
            else playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk);
        }
    }

     private void MovePlayer(Rigidbody2D rb, Transform currentTransform)
    {
        if (isNormal)
        {
            if (!isCrawling && !playerGroundChecker.isOnSlope) rb.velocity = new Vector2(direction * normalSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (playerGroundChecker.isOnSlope && !isCrawling) rb.velocity = new Vector2(-direction * normalSpeed * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, normalSpeed * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (playerGroundChecker.isOnSlope && isCrawling) rb.velocity = new Vector2(-direction * (normalSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, (normalSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (isCrawling) rb.velocity = new Vector2(direction * (normalSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }
        else if (isGhost)
        {
            if (!isCrawling && !playerGroundChecker.isOnSlope) rb.velocity = new Vector2(direction * ghostSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (playerGroundChecker.isOnSlope && !isCrawling) rb.velocity = new Vector2(-direction * ghostSpeed * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, ghostSpeed * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (playerGroundChecker.isOnSlope && isCrawling) rb.velocity = new Vector2(-direction * (ghostSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, (ghostSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (isCrawling) rb.velocity = new Vector2(direction * (ghostSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }

        FlipSpriteBasedOnDirection(currentTransform);
        UpdateOtherTransformObjectPosition();
        UpdateRbFrictionOnSlope(rb);

        ItemAvabileAreaCheck(playerGroundChecker.ghostGroundCheckCollider.position);
    }


    void FlipSpriteBasedOnDirection(Transform transform)
    {
        if (direction > 0f)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
        }
        else if (direction < 0f)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
        }
    }

    //adjusts object positions for transform and cinemachine follow target.
    void UpdateOtherTransformObjectPosition()
    {
        if (isNormal && !isGhost)
        {
            ghostTransform.position = new Vector2(normalTransform.position.x, normalTransform.position.y + 3f);
        }

        else if (isGhost && !isNormal)
        {
            normalTransform.position = new Vector2(ghostTransform.position.x, ghostTransform.position.y - 3f);
        }
    }
    void UpdateRbFrictionOnSlope(Rigidbody2D rb)
    {
        if (playerGroundChecker.isOnSlope && Mathf.Approximately(direction, 0f)) rb.sharedMaterial = playerGroundChecker.fullFriction;
        else rb.sharedMaterial = playerGroundChecker.noFriction;
    }

     void ItemAvabileAreaCheck(Vector2 checkPos)
    {
        RaycastHit2D RaycastFront = Physics2D.Raycast(checkPos, transform.right, 8f, itemLayer);
        if (RaycastFront)
        {
            UsingItem = true;
            Debug.DrawRay(RaycastFront.point, RaycastFront.normal, Color.green);
        }
        else UsingItem = false;
    }

    void ConfinePlayerMovement()
    {
        if (currentConfinerCollider == null) return;
        Vector3 playerPosition = Vector3.zero;
        if (isNormal)
        {
            playerPosition = normalGameObject.transform.position;
        }
        else if (isGhost)
        {
            playerPosition = ghostGameObject.transform.position;
        }

        Bounds bounds = currentConfinerCollider.bounds;

        playerPosition.x = Mathf.Clamp(playerPosition.x, bounds.min.x, bounds.max.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, bounds.min.y, bounds.max.y);

        normalGameObject.transform.position = playerPosition;
        ghostGameObject.transform.position = playerPosition;
    }

    private IEnumerator Dash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;

            //�뽬 �� �������� �ʰ� gravity �� ����
            float originalGravity = ghostRb.gravityScale;
            ghostRb.gravityScale = 0f;

            ghostRb.velocity = new Vector2(direction * dashForce, 0f);
            trailRenderer.emitting = true;

            yield return new WaitForSeconds(dashTime);

            trailRenderer.emitting = false;
            ghostRb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}
