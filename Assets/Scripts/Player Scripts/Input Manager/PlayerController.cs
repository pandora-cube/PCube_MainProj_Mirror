using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected PlayerControls controls;
    protected float direction = 0;
    public float speed = 0;
    private float normalInteractRange = 0.5f;
    private float ghostInteractRange = 1f;

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
    [SerializeField] protected bool isOnSlope = false;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] protected LayerMask groundLayer;
    #endregion

    #region slope variables
    [Header("Slope Variables")]
    [SerializeField] protected LayerMask slopeLayer;
    protected float slopeCheckRadius = 0.2f;
    #endregion

    [Header("Normal Variables")]
    [SerializeField] private GameObject normalGameObejct;
    [SerializeField] private Rigidbody2D normalRb;
    [SerializeField] private PolygonCollider2D normalCollider;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private Transform normalTransform;


    [Header("Ghost Variables")]
    [SerializeField] private GameObject ghostGameObejct;
    [SerializeField] protected Rigidbody2D ghostRb;
    [SerializeField] protected PolygonCollider2D ghostCollider;

    private bool isNormal = true;
    private bool isGhost = false;

    protected virtual void Awake()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
            Move();
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
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, 0.1f, groundLayer);
        RaycastHit2D slopeHit = Physics2D.Raycast(groundCheckCollider.position, new Vector2(0, 0.2f), slopeLayer);

        if (!isCrawling) normalRb.velocity = new Vector2(direction * speed * Time.deltaTime, normalRb.velocity.y);
        else if (isCrawling) normalRb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, normalRb.velocity.y);

        if (Mathf.Approximately(direction, 0f))
        {
            if (!isGrounded) normalRb.velocity = new Vector2(0f, -1f);
            else normalRb.velocity = new Vector2(0f, 0f);
        }

        if (isOnSlope)
        {

        }

        if (direction > 0) normalSprite.flipX = true;
        else normalSprite.flipX = false;

    }
    void Jump()
    {
        if (isGrounded) normalRb.velocity = new Vector2(normalRb.velocity.x, jumpForce);
    }
    void Interact()
    {
        Collider2D[] colldierArray = Physics2D.OverlapCircleAll(normalTransform.position, normalInteractRange);

        foreach (Collider2D collider in colldierArray)
        {
            if (collider.gameObject.CompareTag("Player")) continue; 

            if (collider.TryGetComponent(out Mirror mirror))
            {
                mirror.Interact();
            }
        }
    }
    public void Transform()
    {
        isNormal = !isNormal;
        isGhost = !isGhost;

        normalGameObejct.SetActive(isNormal); ghostGameObejct.SetActive(isGhost);
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
        Gizmos.DrawWireSphere(normalTransform.position, normalInteractRange);
    }
    #endregion
}
