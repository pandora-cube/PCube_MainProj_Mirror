using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
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

    #region jump variables
    [Header("Jump Variables")]
    public float jumpForce = 5f;
    [SerializeField] protected bool isGrounded = false;
    [SerializeField] protected bool isOnSlope = false;
    public Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;
    #endregion

    #region slope variables
    [Header("Slope Variables")]
    [SerializeField] protected LayerMask slopeLayer;
    protected float slopeCheckRadius = 0.2f;
    #endregion

    [Header("Normal Variables")]
    [SerializeField] protected Rigidbody2D normal_rb;
    [SerializeField] protected PolygonCollider2D normal_collider;

    [Header("Ghost Variables")]
    [SerializeField] protected Rigidbody2D ghost_rb;
    [SerializeField] protected PolygonCollider2D ghost_collider;

    [SerializeField] protected SpriteRenderer[] playerSprites;
    void Awake()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
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
        controls.PlayerActions.Smoke.performed += ctx =>
        {
            Smoke();
        };
        #endregion
    }

    protected virtual void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        RaycastHit2D slopeHit = Physics2D.Raycast(groundCheck.position, new Vector2(0, 0.2f), slopeLayer);

        if (!isCrawling) normal_rb.velocity = new Vector2(direction * speed * Time.deltaTime, normal_rb.velocity.y);
        else if (isCrawling) normal_rb.velocity = new Vector2(direction * (speed - crawlSpeedDecrease) * Time.deltaTime, normal_rb.velocity.y);

        if (Mathf.Approximately(direction, 0f))
        {
            if (!isGrounded) normal_rb.velocity = new Vector2(0f, -1f);
            else normal_rb.velocity = new Vector2(0f, 0f);
        }

        if (isOnSlope)
        {

        }

        if (direction > 0) playerSprites[0].flipX = true;
        else playerSprites[0].flipX = false;

    }
    protected virtual void Jump()
    {
        if (isGrounded) normal_rb.velocity = new Vector2(normal_rb.velocity.x, jumpForce);
    }
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
    protected virtual void Interact()
    {

    }
    protected virtual void Attack()
    {

    }
    protected virtual void Teleport()
    {

    }
    protected virtual void Smoke()
    {

    }
}
