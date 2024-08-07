using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    protected float direction = 0;
    public float speed = 0;


    #region crawling variables
    protected float crawlSpeedDecrease = 300f;
    protected Vector2 standColliderSize = new Vector2(1.0f, 1.0f);
    protected Vector2 crawlBoxcolliderSize = new Vector2(1f, 0.3f);
    protected bool isCrawling = false;
    #endregion

    #region dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    [SerializeField] TrailRenderer trailRenderer;

    #endregion

    #region jump variables
    [Header("Jump Variables")]
    public float jumpForce = 5f;
    public bool isGrounded = false;
    [SerializeField] protected LayerMask groundLayer;
    #endregion

    #region slope variables
    [Header("Slope Variables")]
    [SerializeField] protected bool isOnSlope = false;
    [SerializeField] private PhysicsMaterial2D noFiction;
    [SerializeField] private PhysicsMaterial2D fullFiction;
    private Vector2 slopeNormalPrep;

    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    [SerializeField] private float slopeCheckDistanceVert;
    [SerializeField] private float slopeCheckDistanceHori;
    #endregion

    [Header("Common Variables")]
    [SerializeField] private GameObject cameraTransform;

    [Header("Normal Variables")]
    [SerializeField] private GameObject normalGameObejct;
    [SerializeField] private Rigidbody2D normalRb;
    [SerializeField] private PolygonCollider2D normalCollider;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private Transform normalTransform;
    [SerializeField] private Transform normalGroundCheckCollider;
    [SerializeField] private float normalInteractRange = 0.5f;


    [Header("Ghost Variables")]
    [SerializeField] private GameObject ghostGameObejct;
    [SerializeField] protected Rigidbody2D ghostRb;
    [SerializeField] protected PolygonCollider2D ghostCollider;
    [SerializeField] protected SpriteRenderer ghostSprite;
    [SerializeField] private Transform ghostTransform;
    [SerializeField] private Transform ghostGroundCheckCollider;
    [SerializeField] private float ghostInteractRange = 1f;


    public bool isNormal = true;
    public bool isGhost = false;

    #region attack variables
    [Header("Attack Variables")]
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float timeBetweenAttacks = 0.2f;
    private float attackTimeCounter;
    private RaycastHit2D[] hits;

    public bool canReceiveInput;
    public bool inputReceived;
    #endregion

    #region Item variables
    public bool UsingItem = false;
    [SerializeField] protected LayerMask itemLayer;
    #endregion


    private void Update()
    {
        //attackTimeCounter += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (isDashing) return;
                
        GroundCheck();

        if (isNormal && !isGhost)
        {
            SlopeCheck(normalGroundCheckCollider.position);
            MovePlayer(normalRb, normalTransform, ghostTransform, normalGroundCheckCollider.position);
        }
        if (isGhost && !isNormal) 
        {
            SlopeCheck(ghostGroundCheckCollider.position);
            MovePlayer(ghostRb, ghostTransform, normalTransform, ghostGroundCheckCollider.position);
        }
    }

    #region common functions (normal && ghost)

    #region movement functions
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (isDashing) return;

        direction = ctx.ReadValue<float>();
    }

    private void MovePlayer(Rigidbody2D rb, Transform currentTransform, Transform otherTransform, Vector2 groundCheckPosition)
    {
        if (!isCrawling && !isOnSlope) rb.velocity = new Vector2(direction * speed * Time.deltaTime, rb.velocity.y); // normal walk
        else if (isOnSlope && !isCrawling) rb.velocity = new Vector2(-direction * speed * slopeNormalPrep.x * Time.deltaTime, speed * slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
        else if (isOnSlope && isCrawling) rb.velocity = new Vector2(-direction * (speed - crawlSpeedDecrease) * slopeNormalPrep.x * Time.deltaTime, (speed - crawlSpeedDecrease) * slopeNormalPrep.y * -direction * Time.deltaTime);
        else if (isCrawling) rb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y); // craw walk

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
    /// <summary>
    /// move position of inactive object and parent object
    /// </summary>
    void UpdateOtherTransformObjectPosition()
    {
        if (isNormal && !isGhost)
        {
            ghostTransform.position = new Vector3(normalTransform.position.x, normalTransform.position.y + 5f, 0f);
            cameraTransform.transform.position = normalTransform.position; 
        }

        else if (isGhost && !isNormal)
        {
            normalTransform.position = ghostTransform.position;
            cameraTransform.transform.position = ghostTransform.position; 
        }
    }
    void UpdateRbFrictionOnSlope(Rigidbody2D rb)
    {
        if (isOnSlope && Mathf.Approximately(direction, 0f)) rb.sharedMaterial = fullFiction;
        else rb.sharedMaterial = noFiction;
    }

    #endregion

    #region jump functions

    void GroundCheck()
    {
        if (isNormal) isGrounded = Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, groundLayer) || isOnSlope;
        if (isGhost) isGrounded = Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, groundLayer) || isOnSlope;
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isGrounded) return;

        if (isNormal) normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        else if (isGhost) ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        Debug.Log("JUMP!");
    }
    #endregion
    
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (isNormal) 
        {
            Collider2D[] colldierArray = Physics2D.OverlapCircleAll(normalTransform.position, normalInteractRange);
        }
        else if (isGhost)
        {
            Collider2D[] colldierArray = Physics2D.OverlapCircleAll(ghostTransform.position, ghostInteractRange);
        }
        IInteractable interactable = GetInteractableObject();

        if (interactable != null) interactable.Interact(transform);
    }

    /// <summary>
    /// function for finding the nearest interactable object
    /// </summary>
    /// <returns>nearest interactable object found</returns>
    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(normalTransform.position, normalInteractRange);
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

    #region slope check functions
    private void SlopeCheck(Vector2 checkPos)
    {
        SlopeCheckVertical(checkPos);
        SlopeCheckHorizontal(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistanceHori, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistanceHori, groundLayer);

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            isOnSlope = false;
            slopeSideAngle = 0f;
        }
    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistanceVert, groundLayer);
        
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
        if (!isGhost || attackTimeCounter < timeBetweenAttacks) return;

        attackTimeCounter = 0f;
        inputReceived = true;
        canReceiveInput = false;

        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; ++i)
        {
            IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(damageAmount);
                Debug.Log("DAMAGE!!");
            }
        }
    }

    public void InputManager()
    {
        if (!canReceiveInput) canReceiveInput = true;
        else canReceiveInput = false;
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

    #region debugging functions
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(attackTransform.position, attackRange);

    }
    #endregion
}
