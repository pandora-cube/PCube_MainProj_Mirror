using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float direction = 0;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float ghsotSpeed;

    #region CRAWLING VARIABLES
    protected float crawlSpeedDecrease = 300f;
    protected Vector2 standColliderSize = new Vector2(1.0f, 1.0f);
    protected Vector2 crawlBoxcolliderSize = new Vector2(1f, 0.3f);
    protected bool isCrawling = false;
    #endregion

    #region DASH VARIABLES
    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    [SerializeField] TrailRenderer trailRenderer;

    #endregion

    #region JUMP VARIABLES
    [Header("Jump Variables")]
    public float jumpForce = 5f;
    public bool isGrounded = false;
    public bool isHoldingDown = false;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask bridgeLayer;
    #endregion

    #region SLOPE VARIABLES
    [Header("Slope Variables")]
    [SerializeField] private bool isOnSlope = false;
    [SerializeField] private PhysicsMaterial2D noFiction;
    [SerializeField] private PhysicsMaterial2D fullFiction;
    private Vector2 slopeNormalPrep;

    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    [SerializeField] private float slopeCheckDistanceVert;
    [SerializeField] private float slopeCheckDistanceHori;
    #endregion

    #region NORMAL VARIABLES
    [Header("Normal Variables")]
    [SerializeField] public GameObject normalGameObejct;
    [SerializeField] private Rigidbody2D normalRb;
    [SerializeField] private CapsuleCollider2D normalCollider;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private Transform normalTransform;
    [SerializeField] private Transform normalGroundCheckCollider;
    [SerializeField] private Transform normalSlopeCheckCollider;
    [SerializeField] private float normalInteractRange = 0.5f;
    #endregion

    #region GHOST VARIABLS
    [Header("Ghost Variables")]
    [SerializeField] public GameObject ghostGameObejct;
    [SerializeField] protected Rigidbody2D ghostRb;
    [SerializeField] protected CapsuleCollider2D ghostCollider;
    [SerializeField] protected SpriteRenderer ghostSprite;
    [SerializeField] private Transform ghostTransform;
    [SerializeField] private Transform ghostGroundCheckCollider;
    [SerializeField] private Transform ghostSlopeCheckCollider;
    [SerializeField] private float ghostInteractRange = 1f;

    #endregion
    public bool isNormal = true;
    public bool isGhost = false;

    #region ATTACK VARIABLES
    [Header("Attack Variables")]
    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;

    [SerializeField] private float comboResetTime = 1f; // Time allowed between combo attacks
    [SerializeField] private float lastAttackTime;
    [SerializeField] private int comboAttackNumber = 0;
    private bool isAttacking;

    #endregion

    #region ITEM VARIABLES
    public bool UsingItem = false;
    [SerializeField] protected LayerMask itemLayer;
    #endregion

    #region ANIMATION VARIABLES
    private string currentState;

    [SerializeField] private Animator normalAnimator;
    [SerializeField] private Animator ghostAnimator;
    AnimatorStateInfo currentAnimation;

    enum NormalAnimationStates
    {
        normalIdle,
        normalWalk,
    }
    enum GhostAnimationStates
    {
        ghostIdle,
        ghostWalk,
        ghostAttack1,
        ghostAttack2,
        ghostAttack3,
    }
    #endregion

    private void Update()
    {
        //attackTimeCounter += Time.deltaTime;
        if (Time.time - lastAttackTime > comboResetTime)  
        {
            comboAttackNumber = 0;
            ChangeAnimationState(GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
                
        GroundCheck();

        if (isNormal && !isGhost)
        {
            SlopeCheck(normalSlopeCheckCollider.position);
            MovePlayer(normalRb, normalTransform);
        }
        if (isGhost && !isNormal) 
        {
            SlopeCheck(ghostSlopeCheckCollider.position);
            MovePlayer(ghostRb, ghostTransform);
        }
    }

    #region COMMON

    #region MOVEMENT
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (isDashing) return;

        direction = ctx.ReadValue<float>();

        if (isNormal) 
        {
            if (Mathf.Approximately(direction, 0f)) ChangeAnimationState(NormalAnimationStates.normalWalk); //TO-DO: Change to Idle
            else ChangeAnimationState(NormalAnimationStates.normalWalk);
        }
        else if (isGhost)
        {
            if (Mathf.Approximately(direction, 0f)) ChangeAnimationState(GhostAnimationStates.ghostWalk); //TO-DO: Change to Idle
            else ChangeAnimationState(GhostAnimationStates.ghostWalk);
        }
    }

    private void MovePlayer(Rigidbody2D rb, Transform currentTransform)
    {
        if (isNormal)
        {
            if (!isCrawling && !isOnSlope) rb.velocity = new Vector2(direction * normalSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (isOnSlope && !isCrawling) rb.velocity = new Vector2(-direction * normalSpeed * slopeNormalPrep.x * Time.deltaTime, normalSpeed * slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (isOnSlope && isCrawling) rb.velocity = new Vector2(-direction * (normalSpeed - crawlSpeedDecrease) * slopeNormalPrep.x * Time.deltaTime, (normalSpeed - crawlSpeedDecrease) * slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (isCrawling) rb.velocity = new Vector2(direction * (normalSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }
        else if (isGhost)
        {
            if (!isCrawling && !isOnSlope) rb.velocity = new Vector2(direction * ghsotSpeed * Time.deltaTime, rb.velocity.y); // normal walk
            else if (isOnSlope && !isCrawling) rb.velocity = new Vector2(-direction * ghsotSpeed * slopeNormalPrep.x * Time.deltaTime, ghsotSpeed * slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (isOnSlope && isCrawling) rb.velocity = new Vector2(-direction * (ghsotSpeed - crawlSpeedDecrease) * slopeNormalPrep.x * Time.deltaTime, (ghsotSpeed - crawlSpeedDecrease) * slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (isCrawling) rb.velocity = new Vector2(direction * (ghsotSpeed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk
        }
        
        FlipSpriteBasedOnDirection(currentTransform);
        UpdateOtherTransformObjectPosition();
        UpdateRbFrictionOnSlope(rb);
        
        ItemAvabileAreaCheck(ghostGroundCheckCollider.position);
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
        if (isOnSlope && Mathf.Approximately(direction, 0f)) rb.sharedMaterial = fullFiction;
        else rb.sharedMaterial = noFiction;
    }

    #endregion

    #region JUMP

    void GroundCheck()
    {
        if (isNormal) isGrounded = Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, platformLayer) || Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, bridgeLayer) || isOnSlope;
        if (isGhost) isGrounded = Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, platformLayer) || Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, bridgeLayer) || isOnSlope;
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isGrounded) return;

        if (isHoldingDown) StartCoroutine(JumpDownThroughPlatform());
        else
        {
            if (isNormal) normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            else if (isGhost) ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    IEnumerator JumpDownThroughPlatform()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), true);

        if (isNormal) normalRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
        else if (isGhost) ghostRb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bridge"), false);
    }

    public void OnHoldDown(InputAction.CallbackContext ctx)
    {
        Debug.Log("하향점프");
        if (ctx.started) isHoldingDown = true;
        else if (ctx.canceled) isHoldingDown = false;
    }
    #endregion
    
    #region INTERACTION

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        IInteractable interactable = GetInteractableObject();

        if (interactable != null) interactable.Interact(transform);
        else Debug.Log("interactable is null!");
    }

    /// <summary>
    /// function for finding the nearest interactable object
    /// </summary>
    /// <returns>nearest interactable object found</returns>
    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider2D[] colliderArray = null;
        if (isNormal) 
        {
            colliderArray = Physics2D.OverlapCircleAll(normalTransform.position, normalInteractRange);
        }
        else if (isGhost)
        {
            colliderArray = Physics2D.OverlapCircleAll(ghostTransform.position, ghostInteractRange);
        }
        foreach (Collider2D collider in colliderArray)
        {
            if (collider.gameObject.CompareTag("Player")) continue;

            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null) closestInteractable = interactable;
            else
            {
                if (Vector2.Distance(normalTransform.position, interactable.GetTransform().position) <
                    Vector2.Distance(normalTransform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
    public void Transform()
    {
        isNormal = !isNormal;
        isGhost = !isGhost;

        normalGameObejct.SetActive(isNormal); ghostGameObejct.SetActive(isGhost);
    }
    #endregion 

    #region SLOPE CHECK 
    private void SlopeCheck(Vector2 checkPos)
    {
        SlopeCheckVertical(checkPos);
        SlopeCheckHorizontal(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistanceHori, platformLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistanceHori, platformLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
            Debug.DrawRay(checkPos, transform.right, Color.red);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            Debug.DrawRay(checkPos, -transform.right, Color.red);
        }
        else
        {
            isOnSlope = false;
            slopeSideAngle = 0f;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistanceVert, platformLayer);
        
        if (hit)
        {
            slopeNormalPrep = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle) isOnSlope = true;

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPrep, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }
    #endregion

    #endregion

    #region player only functions
    public void OnCrawl(InputAction.CallbackContext ctx)
    {
        isCrawling = !isCrawling;

        if (isCrawling)
        {
            //normal_collider.size = crawlBoxcolliderSize;
        }
        else
        {
            //boxCollider.size = standColliderSize;
        }
        Debug.Log(isCrawling);
    }

    #endregion

    #region ghost only functions
    public void Teleport()
    {

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
    void Smoke()
    {

    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!isGhost || !ctx.started) return; 

        isAttacking = true;
        lastAttackTime = Time.time;

        comboAttackNumber++;

        
        if (comboAttackNumber > 3)  comboAttackNumber = 1;

        if (CheckIfAttackAnimationHasEnded()) TriggerAttackAnimation();
        else comboAttackNumber--;
    }

    private bool CheckIfAttackAnimationHasEnded()
    {
        currentAnimation = ghostAnimator.GetCurrentAnimatorStateInfo(0);
        if (!(currentAnimation.IsName("ghostAttack1") || currentAnimation.IsName("ghostAttack2") || currentAnimation.IsName("ghostAttack3"))) return true;
        if (currentAnimation.normalizedTime >= 1.0f && !ghostAnimator.IsInTransition(0)) return true;
        else return false;
    }
    private void TriggerAttackAnimation()
    {
        switch (comboAttackNumber)
        {
            case 1: ChangeAnimationState(GhostAnimationStates.ghostAttack1); break;
            case 2: ChangeAnimationState(GhostAnimationStates.ghostAttack2); break;  
            case 3: ChangeAnimationState(GhostAnimationStates.ghostAttack3); break;
        }     
    }
    #endregion

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

    #region ANIMATION
    private void ChangeAnimationState (GhostAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;
        
        ghostAnimator.Play(newState);
        currentState = newState;
    }

    private void ChangeAnimationState(NormalAnimationStates animationStates)
    {
        string newState = animationStates.ToString(); 
        if (currentState == newState) return;

        normalAnimator.Play(newState);
        currentState = newState;
    }

    #endregion
    #region DEBUGGING
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ghostTransform.position, ghostInteractRange);
    }
    #endregion
}
