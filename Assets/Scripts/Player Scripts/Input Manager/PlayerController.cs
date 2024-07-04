using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    float direction = 0;
    public float speed = 0;

    #region crawling variables
    private float crawlSpeedDecrease = 300f;
    private Vector2 standColliderSize = new Vector2(1.0f, 1.0f);
    private Vector2 crawlBoxcolliderSize = new Vector2(1f, 0.3f);
    private bool isCrawling = false;
    #endregion

    #region jump variables
    public float jumpForce = 5f;
    bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayer;
    #endregion

    #region dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashForce = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 5.0f;
    #endregion

    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        InitializeVariables();
    }
    void FixedUpdate()
    {
        if (isDashing) return; //대쉬 중 이동 금지
        Move();
    }

    void InitializeVariables()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

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
        controls.PlayerActions.Interact.performed += ctx =>
        {
            Interact();
        };
        controls.PlayerActions.Attack.performed += ctx =>
        {
            Attack();
        };
        controls.PlayerActions.Teleport.performed += ctx =>
        {
            Teleport();
        };
        controls.PlayerActions.Dash.performed += ctx =>
        {
            StartCoroutine(Dash());
        };
        controls.PlayerActions.Smoke.performed += ctx =>
        {
            Smoke();
        };
        #endregion
    }
    void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (!isCrawling) rb.velocity = new Vector2(direction * speed * Time.deltaTime, rb.velocity.y);
        else if (isCrawling) rb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, rb.velocity.y);
    }
    void Jump()
    {
       if(isGrounded) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    void Crawl()
    {
        isCrawling = !isCrawling;

        if (isCrawling)
        {
            boxCollider.size = crawlBoxcolliderSize;
        }
        else
        {
            boxCollider.size = standColliderSize;
        }
        Debug.Log(isCrawling);
    }
    void Interact()
    {
            
    }
    void Attack()
    {

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
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(direction * dashForce, 0f);
            trailRenderer.emitting = true;

            yield return new WaitForSeconds(dashTime);

            trailRenderer.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
    void Smoke()
    {

    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        InteractableObject interactableObject = collision.gameObject.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            if (interactableObject.ObjectName == "Mirror")
            {
                Debug.Log("MIRROR!!!");
            }
        }
    }
}
