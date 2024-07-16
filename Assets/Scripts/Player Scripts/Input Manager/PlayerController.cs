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
    [SerializeField] protected bool isGrounded = false;
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


    [SerializeField] private bool isNormal = true;
    [SerializeField] private bool isGhost = false;

    #region attack variables
    [Header("Attack Variables")]
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    private RaycastHit2D[] hits;
    #endregion

    protected virtual void Awake()
    {
        InitializeVariables();
    }


    private void FixedUpdate()
    {
        GroundCheck();
        if (isNormal) SlopeCheck(normalGroundCheckCollider.position);
        if (isGhost) SlopeCheck(ghostGroundCheckCollider.position);


    }
    protected virtual void InitializeVariables()
    {
 

        //input manager 각 action맵에 대한 구독
        #region input actions
        //controls.PlayerActions.Attack.performed += ctx =>
        //{
        //    Attack();
        //};
        //controls.PlayerActions.Teleport.performed += ctx =>
        //{
        //    Teleport();
        //};
        //controls.PlayerActions.Smoke.performed += ctx =>
        //{
        //    Smoke();
        //};

        #endregion
    }

    #region common functions (normal && ghost)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (isDashing) return;

        direction = ctx.ReadValue<float>();
        if (isNormal)
        {

            if (!isCrawling && !isOnSlope) normalRb.velocity = new Vector2(direction * speed * Time.deltaTime, normalRb.velocity.y); // normal walk
            else if (isOnSlope &&!isCrawling) normalRb.velocity = new Vector2(-direction * speed * slopeNormalPrep.x * Time.deltaTime, speed * slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (isOnSlope && isCrawling) normalRb.velocity = new Vector2(-direction * (speed- crawlSpeedDecrease) * slopeNormalPrep.x * Time.deltaTime, (speed-crawlSpeedDecrease) * slopeNormalPrep.y * -direction * Time.deltaTime);
            else if (isCrawling) normalRb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, normalRb.velocity.y); // craw walk

            //flip sprite based on direction of travel
            if (direction > 0f)
            {
                Vector3 newScale = normalTransform.localScale;
                newScale.x = 1;
                normalTransform.localScale = newScale;
            }
            else if (direction < 0f)
            {
                Vector3 newScale = normalTransform.localScale;
                newScale.x = -1;
                normalTransform.localScale = newScale;
            }
            //update position of ghost object when normal
            ghostTransform.position = new Vector3(normalTransform.position.x, normalTransform.position.y + 5f, 0f);

            if (isOnSlope && Mathf.Approximately(direction, 0f)) normalRb.sharedMaterial = fullFiction;
            else normalRb.sharedMaterial = noFiction;
        }

        if (isGhost)
        {
            SlopeCheck(ghostGroundCheckCollider.position);

            if (!isCrawling && !isOnSlope) ghostRb.velocity = new Vector2(direction * speed * Time.deltaTime, ghostRb.velocity.y);
            else if (isOnSlope && !isCrawling) ghostRb.velocity = new Vector2(-direction * speed * slopeNormalPrep.x * Time.deltaTime, speed * slopeNormalPrep.y * Time.deltaTime); // slope walk
            else if (isOnSlope && isCrawling) ghostRb.velocity = new Vector2(-direction * (speed-crawlSpeedDecrease) * slopeNormalPrep.x * Time.deltaTime, (speed-crawlSpeedDecrease) * slopeNormalPrep.y * Time.deltaTime);
            else if (isCrawling) ghostRb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, ghostRb.velocity.y);

            //flip sprite based on direction of travel
            if (direction > 0f)
            {
                Vector3 newScale = ghostTransform.localScale;
                newScale.x = 1;
                ghostTransform.localScale = newScale;
            }
            else if (direction < 0f)
            {
                Vector3 newScale = ghostTransform.localScale;
                newScale.x = -1;
                ghostTransform.localScale = newScale;
            }

            //update position of ghost object when normal
            normalTransform.position = ghostTransform.position;

            if (isOnSlope && Mathf.Approximately(direction, 0f)) ghostRb.sharedMaterial = fullFiction;
            else ghostRb.sharedMaterial = noFiction;
        }
    }

    void GroundCheck()
    {
        if (isNormal) isGrounded = Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, groundLayer) || isOnSlope;
        if (isGhost) isGrounded = Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, groundLayer) || isOnSlope;

    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            if (isNormal)
            {
                Debug.Log("before:" + normalRb.velocity);
                normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                Debug.Log("after:" + normalRb.velocity);
            }

            else if (isGhost) ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
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

            //대쉬 중 떨어지지 않게 gravity 값 변경
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
        if (!isGhost) return;

        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; ++i)
        {
            IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.Damage(damageAmount);
            }
        }
    }
    #endregion



    #region debugging functions
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);

    }
    #endregion
}
