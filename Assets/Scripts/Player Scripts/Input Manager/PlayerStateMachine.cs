using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    #region CRAWLING VARIABLES
    protected float crawlSpeedDecrease = 300f;
    protected Vector2 standColliderSize = new Vector2(1.0f, 1.0f);
    protected Vector2 crawlBoxcolliderSize = new Vector2(1f, 0.3f);
    protected bool isCrawling = false;
    #endregion

    #region NORMAL VARIABLES
    [Header("Normal Variables")]
    public GameObject normalGameObject;
    [SerializeField] protected Rigidbody2D normalRb;
    [SerializeField] private CapsuleCollider2D normalCollider;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] protected Transform normalTransform;
    #endregion

    #region GHOST VARIABLS
    [Header("Ghost Variables")]
    public GameObject ghostGameObject;
    [SerializeField] protected Rigidbody2D ghostRb;
    [SerializeField] protected BoxCollider2D ghostCollider;
    [SerializeField] protected SpriteRenderer ghostSprite;
    [SerializeField] protected Transform ghostTransform;

    #endregion
    public bool isNormal = true;
    public bool isGhost = false;

    #region ITEM VARIABLES
    [Header("Item Variables")]
    public bool UsingItem = false;
    [SerializeField] protected LayerMask itemLayer;
    #endregion

    #region OTHER VARIABLES
    [Header("Other Variables")]
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] protected Inventory inventory;
    private PlayerInput playerInput;
    [HideInInspector] public bool canMove = true;
    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }


    private void Update()
    {

        playerInput.enabled = canMove; //disable player input when dialog is happening

    }

    #region GHOST ONLY
    public void Teleport()
    {

    }


    void Smoke()
    {

    }

    #endregion //GHOST ONLY

   


    #region DEBUGGING
    private void OnDrawGizmos()
    {

    }
    #endregion
}
