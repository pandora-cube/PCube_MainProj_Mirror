using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontalMovement : MonoBehaviour
{
    #region MOVEMENT VARIABLES
    [SerializeField] private float direction = 0;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float ghostSpeed;
    [HideInInspector] public PolygonCollider2D currentConfinerCollider;
    #endregion

    #region DASH VARIABLES
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    [SerializeField] TrailRenderer trailRenderer;
    #endregion

    #region CRAWLING VARIABLES
    protected float crawlSpeedDecrease = 300f;
    protected Vector2 standColliderSize = new Vector2(1.0f, 1.0f);
    protected Vector2 crawlBoxcolliderSize = new Vector2(1f, 0.3f);
    #endregion

    PlayerAnimationController playerAnimationController;
    PlayerGroundChecker playerGroundChecker;
    PlayerComponents playerComponents;

    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;

    void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerGroundChecker = GetComponent<PlayerGroundChecker>();
        playerComponents = GetComponent<PlayerComponents>();
    }

    void Update()
    {
        if (!PlayerState.canMove)
        {
            direction = 0f;
            playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalIdle);
        }
    }


    void FixedUpdate()
    {
        if (isDashing) return;

        if (PlayerState.isNormal && !PlayerState.isGhost)
        {
            MovePlayer(playerComponents.normalRb, playerComponents.normalTransform);
            ConfinePlayerMovement();
        }
        else if (PlayerState.isGhost && PlayerState.isNormal)
        {
            MovePlayer(playerComponents.ghostRb, playerComponents.ghostTransform);
            ConfinePlayerMovement();
        }
    }

    public void OnMove(InputValue value)
    {
        if (isDashing) return;

        direction = value.Get<float>();

        if (PlayerState.isNormal)
        {
            if (Mathf.Approximately(direction, 0f)) playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalIdle);
            else playerAnimationController.ChangeAnimationState(PlayerAnimationController.NormalAnimationStates.normalWalk);
        }
        else if (PlayerState.isGhost)
        {
            if (Mathf.Approximately(direction, 0f)) playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
            else playerAnimationController.ChangeAnimationState(PlayerAnimationController.GhostAnimationStates.ghostWalk);
        }
    }

    private void MovePlayer(Rigidbody2D rb, Transform currentTransform)
    {
        if (PlayerState.isNormal)
        {
            if (!PlayerState.isCrawling && PlayerState.isOnSlope) rb.velocity = new Vector2(direction * normalSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (PlayerState.isOnSlope && !PlayerState.isCrawling) rb.velocity = new Vector2(-direction * normalSpeed * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, normalSpeed * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (PlayerState.isOnSlope && PlayerState.isCrawling) rb.velocity = new Vector2(-direction * (normalSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, (normalSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (PlayerState.isCrawling) rb.velocity = new Vector2(direction * (normalSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }
        else if (PlayerState.isGhost)
        {
            if (!PlayerState.isCrawling && !PlayerState.isOnSlope) rb.velocity = new Vector2(direction * ghostSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (PlayerState.isOnSlope && !PlayerState.isCrawling) rb.velocity = new Vector2(-direction * ghostSpeed * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, ghostSpeed * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (PlayerState.isOnSlope && PlayerState.isCrawling) rb.velocity = new Vector2(-direction * (ghostSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.x * Time.deltaTime, (ghostSpeed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (PlayerState.isCrawling) rb.velocity = new Vector2(direction * (ghostSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }

        FlipSpriteBasedOnDirection(currentTransform);
        UpdateOtherTransformObjectPosition();
        UpdateRbFrictionOnSlope(rb);

        //ItemAvabileAreaCheck(playerGroundChecker.ghostGroundCheckCollider.position);
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
        if (PlayerState.isNormal && !PlayerState.isGhost)
        {
            playerComponents.ghostTransform.position = new Vector2(playerComponents.normalTransform.position.x, playerComponents.normalTransform.position.y + 3f);
        }

        else if (PlayerState.isGhost && !PlayerState.isNormal)
        {
            playerComponents.normalTransform.position = new Vector2(playerComponents.ghostTransform.position.x, playerComponents.ghostTransform.position.y - 3f);
        }
    }
    void UpdateRbFrictionOnSlope(Rigidbody2D rb)
    {
        if (PlayerState.isOnSlope && Mathf.Approximately(direction, 0f)) rb.sharedMaterial = playerGroundChecker.fullFriction;
        else rb.sharedMaterial = playerGroundChecker.noFriction;
    }

    // void ItemAvabileAreaCheck(Vector2 checkPos)
    // {
    //     RaycastHit2D RaycastFront = Physics2D.Raycast(checkPos, transform.right, 8f, itemLayer);
    //     if (RaycastFront)
    //     {
    //         UsingItem = true;
    //         Debug.DrawRay(RaycastFront.point, RaycastFront.normal, Color.green);
    //     }
    //     else UsingItem = false;
    // }

    void ConfinePlayerMovement()
    {
        if (currentConfinerCollider == null) return;
        Vector3 playerPosition = Vector3.zero;
        if (PlayerState.isNormal)
        {
            playerPosition = playerComponents.normalGameObject.transform.position;
        }
        else if (PlayerState.isGhost)
        {
            playerPosition = playerComponents.ghostGameObject.transform.position;
        }

        Bounds bounds = currentConfinerCollider.bounds;

        playerPosition.x = Mathf.Clamp(playerPosition.x, bounds.min.x, bounds.max.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, bounds.min.y, bounds.max.y);

        playerComponents.normalGameObject.transform.position = playerPosition;
        playerComponents.ghostGameObject.transform.position = playerPosition;
    }

    private IEnumerator Dash()
    {
        if (PlayerState.canDash)
        {
            PlayerState.canDash = false;
            isDashing = true;

            //�뽬 �� �������� �ʰ� gravity �� ����
            float originalGravity = playerComponents.ghostRb.gravityScale;
            playerComponents.ghostRb.gravityScale = 0f;

            playerComponents.ghostRb.velocity = new Vector2(direction * dashForce, 0f);
            trailRenderer.emitting = true;

            yield return new WaitForSeconds(dashTime);

            trailRenderer.emitting = false;
            playerComponents.ghostRb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            PlayerState.canDash = true;
        }
    }
}
