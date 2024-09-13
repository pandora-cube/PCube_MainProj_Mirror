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
    [SerializeField] private float baseSpeed = 2f; 
    [SerializeField] private float maxSpeed = 5f; 
    [SerializeField] private float slowDownDistance = 2f; 
    [SerializeField] private float dynamicSpeed;

    void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
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
    }

    void FollowPlayer(GameObject targetPlayer)
    {
        float distance = Vector3.Distance(gameObject.transform.position, targetPlayer.transform.position);
        
        Vector3 targetPosition = targetPlayer.transform.position + new Vector3(-PLAYER_TO_GLASYA_DISTANCE, 0f, 0f);

        dynamicSpeed = distance - PLAYER_TO_GLASYA_DISTANCE;
        
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, dynamicSpeed * Time.deltaTime);
    }
}
