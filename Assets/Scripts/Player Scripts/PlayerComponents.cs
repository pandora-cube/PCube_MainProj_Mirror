using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents instance;
    private static PlayerStateMachine PlayerState => PlayerStateMachine.instance;


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

    [Header("Slope Variables")]
    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;

    #region OTHER VARIABLES
    private PlayerInput playerInput;
    private InputActionAsset inputActionAsset;
    #endregion


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputActionAsset = playerInput.actions;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public Transform GetPlayerTransform()
    {
        if (PlayerState.isNormal) return normalTransform;
        else return ghostTransform;
    }

    public void EnableOnlyAction(params string[] actionNames)
    {
        foreach (var action in inputActionAsset)
        {
            if (System.Array.Exists(actionNames, name => name == action.name))
            {
                action.Enable(); 
            }
            else
            {
                action.Disable(); 
            }
        }
    }

    public void EnableAllActons()
    {
        foreach (var action in inputActionAsset)
        {
            action.Enable();
        }
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
