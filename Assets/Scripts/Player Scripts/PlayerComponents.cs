using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponents : MonoBehaviour
{
    #region NORMAL VARIABLES
    [Header("Normal Variables")]
    public GameObject normalGameObject;
    public Rigidbody2D normalRb;
    public CapsuleCollider2D normalCollider;
    public SpriteRenderer normalSprite;
    public Transform normalTransform;
    #endregion

    #region GHOST VARIABLS
    [Header("Ghost Variables")]
    public GameObject ghostGameObject;
    public Rigidbody2D ghostRb;
    public BoxCollider2D ghostCollider;
    public SpriteRenderer ghostSprite;
    public Transform ghostTransform;

    #endregion

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
    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }


    private void Update()
    {

        playerInput.enabled = PlayerStateMachine.instance.canMove; //disable player input when dialog is happening

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
