using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected PlayerControls controls;
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

    protected virtual void Awake()
    {
        InitializeVariables();
    }


    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
            
    }
    protected virtual void InitializeVariables()
    {
        //input manager 초기화
        controls = new PlayerControls();
        controls.Enable();


        //input manager 각 action맵에 대한 구독
        #region input actions
        controls.PlayerActions.Movement.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };
        controls.PlayerActions.Jump.performed += ctx =>
        {
            Jump();
        };
        controls.PlayerActions.Crawl.performed += ctx =>
        {
            Crawl();
        };

        controls.PlayerActions.Attack.performed += ctx =>
        {
            Attack();
        };
        controls.PlayerActions.Teleport.performed += ctx =>
        {
            Teleport();
        };
        controls.PlayerActions.Smoke.performed += ctx =>
        {
            Smoke();
        };

        controls.PlayerActions.Interact.performed += ctx =>
        {
            Interact();
        };
        #endregion
    }

    #region common functions (normal && ghost)
    void Move()
    {
        if (isNormal)
        {
            isGrounded = Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, groundLayer);
            //RaycastHit2D slopeHit = Physics2D.Raycast(normalGroundCheckCollider.position, new Vector2(0, 0.2f), slopeLayer);
            SlopeCheck(normalGroundCheckCollider.position);

            if (!isCrawling && !isOnSlope) normalRb.velocity = new Vector2(direction * speed * Time.deltaTime, normalRb.velocity.y); // normal walk
            else if (isOnSlope) normalRb.velocity = new Vector2(-direction * speed * slopeNormalPrep.x * Time.deltaTime, speed * slopeNormalPrep.y * -direction * Time.deltaTime); // slope walk
            else if (isCrawling) normalRb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, normalRb.velocity.y); // craw walk

            //flip sprite based on direction of travel
            if (direction > 0f) normalSprite.flipX = true;
            else if (direction < 0f) normalSprite.flipX = false;

            //update position of ghost object when normal
            ghostTransform.position = new Vector3(normalTransform.position.x, normalTransform.position.y + 0.8f, 0f);

            if (isOnSlope && Mathf.Approximately(direction, 0f)) normalRb.sharedMaterial = fullFiction;
            else normalRb.sharedMaterial = noFiction;
        }

        if (isGhost)
        {
            isGrounded = Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, groundLayer);
            //RaycastHit2D slopeHit = Physics2D.Raycast(ghostGroundCheckCollider.position, new Vector2(0, 0.2f), slopeLayer);
            SlopeCheck(ghostGroundCheckCollider.position);

            if (!isCrawling && !isOnSlope) ghostRb.velocity = new Vector2(direction * speed * Time.deltaTime, ghostRb.velocity.y);
            else if (isOnSlope) ghostRb.velocity = new Vector2(-direction * speed * slopeNormalPrep.x * Time.deltaTime, speed * slopeNormalPrep.y * Time.deltaTime); // slope walk
            else if (isCrawling) ghostRb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, ghostRb.velocity.y);

            //flip sprite based on direction of travel
            if (direction > 0f) ghostSprite.flipX = false;
            else if (direction < 0f) ghostSprite.flipX = true;

            //update position of plyaer object when normal
            normalTransform.position = ghostTransform.position;

            if (isOnSlope && Mathf.Approximately(direction, 0f)) ghostRb.sharedMaterial = fullFiction;
            else ghostRb.sharedMaterial = noFiction;
        }

        if (Mathf.Approximately(direction, 0f))
        {
            if (!isGrounded) normalRb.velocity = new Vector2(0f, -1f);
            else normalRb.velocity = new Vector2(0f, 0f);
        }

    }
    void Jump()
    {
        if (isGrounded)
        {
            if (isNormal) normalRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            else if (isGhost) ghostRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
    void Interact()
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

    #region player only functions
    protected virtual void Crawl()
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

    void Attack()
    {

    }
    #endregion



    #region debugging functions
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(ghostTransform.position, ghostInteractRange);
        #endregion
    }
}
