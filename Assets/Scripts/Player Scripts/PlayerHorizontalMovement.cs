using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerHorizontalMovement : MonoBehaviour
{
    #region MOVEMENT VARIABLES
    [SerializeField] private float direction = 0;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float ghostSpeed;
    [HideInInspector] public PolygonCollider2D currentConfinerCollider;
    #endregion

    #region DASH VARIABLES
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
        if (PlayerState.isTakingDamage) return;

        if (PlayerState.isDashing || PlayerState.isAttacking)
        {
            playerComponents.normalRb.velocity = new Vector2(0f, 0f);
            playerComponents.ghostRb.velocity = new Vector2(0f, 0f);
            return;
        }

        if (PlayerState.isNormal)
        {
            MovePlayer(playerComponents.normalRb, playerComponents.normalTransform);
            ConfinePlayerMovement();
        }
        else if (PlayerState.isGhost)
        {
            MovePlayer(playerComponents.ghostRb, playerComponents.ghostTransform);
            ConfinePlayerMovement();
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (PlayerState.isDashing || PlayerState.isAttacking || PlayerState.isTakingDamage)
        {
            direction = 0f;
            return;
        }

        direction = value.ReadValue<float>();

        TriggerWalkAnimation();
    }

    private void TriggerWalkAnimation()
    {
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
        float speed = DecidePlayerSpeed();

        if (PlayerState.isNormal)
        {
            if (!PlayerState.isOnSlope && !PlayerState.isCrawling) rb.velocity = new Vector2(direction * speed, rb.velocity.y); // normal walk
            else if (PlayerState.isOnSlope && !PlayerState.isCrawling) rb.velocity = new Vector2(-direction * speed * playerGroundChecker.slopeNormalPrep.x, speed * playerGroundChecker.slopeNormalPrep.y * -direction); // slope walk
            else if (PlayerState.isOnSlope && PlayerState.isCrawling) rb.velocity = new Vector2(-direction * (speed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.x, (speed - crawlSpeedDecrease) * playerGroundChecker.slopeNormalPrep.y * -direction);
            else if (!PlayerState.isOnSlope && PlayerState.isCrawling) rb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease), rb.velocity.y); // craw walk
        }
        else if (PlayerState.isGhost)
        {
            if (!PlayerState.isOnSlope) rb.velocity = new Vector2(direction * speed, rb.velocity.y); // normal walk
            else if (PlayerState.isOnSlope) rb.velocity = new Vector2(-direction * speed * playerGroundChecker.slopeNormalPrep.x, speed * playerGroundChecker.slopeNormalPrep.y * -direction); // slope walk
        }

        FlipSpriteBasedOnDirection(currentTransform);
        //UpdateOtherTransformObjectPosition();
        UpdateRbFrictionOnSlope(rb);

        //ItemAvabileAreaCheck(playerGroundChecker.ghostGroundCheckCollider.position);
    }

    //Decide player speed based on current player state (normal, crawling, or ghost)
    private float DecidePlayerSpeed()
    {
        //only normal player can crawl.
        if (PlayerState.isNormal)
        {
            if (PlayerState.isCrawling) return normalSpeed - crawlSpeedDecrease;
            else return normalSpeed;
        }
        else return ghostSpeed;
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
    public void UpdateOtherTransformObjectPosition()
    {
        Debug.Log("HERE!");
        if (PlayerState.isNormal && !PlayerState.isGhost)
        {
            Debug.Log("YEA!");
            playerComponents.normalTransform.position = new Vector2(playerComponents.ghostTransform.position.x, playerComponents.ghostTransform.position.y - 3f);

        }
        else if (PlayerState.isGhost && !PlayerState.isNormal)
        {
            playerComponents.ghostTransform.position = new Vector2(playerComponents.normalTransform.position.x, playerComponents.normalTransform.position.y + 3f);

        }
    }

    void UpdateRbFrictionOnSlope(Rigidbody2D rb)
    {
        if (PlayerState.isOnSlope && Mathf.Approximately(direction, 0f)) rb.sharedMaterial = PlayerComponents.instance.fullFriction;
        else rb.sharedMaterial = PlayerComponents.instance.noFriction;
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
            PlayerState.isDashing = true;

            //�뽬 �� �������� �ʰ� gravity �� ����
            float originalGravity = playerComponents.ghostRb.gravityScale;
            playerComponents.ghostRb.gravityScale = 0f;

            playerComponents.ghostRb.velocity = new Vector2(direction * dashForce, 0f);
            trailRenderer.emitting = true;

            yield return new WaitForSeconds(dashTime);

            trailRenderer.emitting = false;
            playerComponents.ghostRb.gravityScale = originalGravity;
            PlayerState.isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            PlayerState.canDash = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Enemy"))
        {
            PlayerComponents.instance.ghostRb.sharedMaterial = PlayerComponents.instance.fullFriction;
        }
    }
}
