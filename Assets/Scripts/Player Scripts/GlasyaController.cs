using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlasyaController : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] GameObject playerNormalObject;
    [SerializeField] GameObject playerGhostObject;
    private const float PLAYER_TO_GLASYA_DISTANCE = 6f;
    [SerializeField] private float lastMoveTime;
    [SerializeField] private float timeElapsed;
    [SerializeField] private float movementDelayTime;
    [SerializeField] private float slowDownDistance = 2f; 
    [SerializeField] private float dynamicSpeed;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        timeElapsed = 0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= movementDelayTime)
        {
            if (playerController.isNormal)
            {
                FollowPlayer(playerNormalObject);
            }
            else if (playerController.isGhost)
            {
                FollowPlayer(playerGhostObject);
            }
        }

        if (playerNormalObject.transform.localScale.x < 0 || playerGhostObject.transform.localScale.x < 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
        }
    }

    void FollowPlayer(GameObject targetPlayer)
    {
        bool isPlayerFacingLeft = targetPlayer.transform.localScale.x < 0;

        float distance = Vector3.Distance(gameObject.transform.localPosition, targetPlayer.transform.localPosition);
    
        Vector3 targetPosition;
        
        if (isPlayerFacingLeft) 
        {
            targetPosition = new Vector3(targetPlayer.transform.localPosition.x - PLAYER_TO_GLASYA_DISTANCE, targetPlayer.transform.localPosition.y, gameObject.transform.localPosition.z);
        }
        else
        {
            targetPosition = new Vector3(targetPlayer.transform.localPosition.x + PLAYER_TO_GLASYA_DISTANCE, targetPlayer.transform.localPosition.y, gameObject.transform.localPosition.z);
        }

        dynamicSpeed = distance - PLAYER_TO_GLASYA_DISTANCE;
        
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, targetPosition, dynamicSpeed * Time.deltaTime);
    }
}
