using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    #region SLOPE VARIABLES
    
    [HideInInspector] public Vector2 slopeNormalPrep;

    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    [SerializeField] private float slopeCheckDistanceVert;
    [SerializeField] private float slopeCheckDistanceHori;

    public LayerMask platformLayer;
    public LayerMask bridgeLayer;

    [SerializeField] private Transform normalGroundCheckCollider;
    [SerializeField] private Transform normalSlopeCheckCollider;

    public Transform ghostGroundCheckCollider;
    [SerializeField] private Transform ghostSlopeCheckCollider;

    #endregion

    PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    PlayerComponents playerComponents;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
    }

    private void FixedUpdate()
    {
        GroundCheck();

        if (PlayerState.isNormal && PlayerState.isGhost) SlopeCheck(normalSlopeCheckCollider.position);
    }
    void GroundCheck()
    {
        if (PlayerState.isNormal) PlayerState.isGrounded = Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, platformLayer) || Physics2D.OverlapCircle(normalGroundCheckCollider.position, 0.1f, bridgeLayer) || PlayerState.isOnSlope;
        if (PlayerState.isGhost) PlayerState.isGrounded = Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, platformLayer) || Physics2D.OverlapCircle(ghostGroundCheckCollider.position, 0.3f, bridgeLayer) || PlayerState.isOnSlope;
    }

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
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
            //Debug.Log("slopeSideAngle : " + slopeSideAngle);

            if (slopeSideAngle <= 60) PlayerState.isOnSlope = true;
            Debug.DrawRay(checkPos, transform.right, Color.red);
        }
        else if (slopeHitBack)
        {
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            //Debug.Log("slopeSideAngle : " + slopeSideAngle);

            if (slopeSideAngle <= 60) PlayerState.isOnSlope = true;
            Debug.DrawRay(checkPos, -transform.right, Color.red);
        }
        else
        {
            //Debug.Log("Not Found Slope");
            PlayerState.isOnSlope = false;
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

            if (slopeDownAngle != lastSlopeAngle) PlayerState.isOnSlope = true;

            //Debug.Log("slopeDownAnlge : " + slopeDownAngle);

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPrep, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }
    #endregion
}
