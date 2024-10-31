using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class GlasyaController : MonoBehaviour
{
    private PlayerStateMachine PlayerState => PlayerStateMachine.instance;
    private PlayerComponents playerComponents;

    private const float PLAYER_TO_GLASYA_DISTANCE = 6f;

    [SerializeField] private float lastMoveTime;
    [SerializeField] private float timeElapsed;
    [SerializeField] private float movementDelayTime;
    [SerializeField] private float dynamicSpeed;

    void Awake()
    {
        GameObject childObject = GameObject.FindGameObjectWithTag("Player");

        if (childObject == null) Debug.LogError("No object with tag PLAYER found!");
        
        Transform currentTransform = childObject.transform;

        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }
        playerComponents = currentTransform.gameObject.GetComponent<PlayerComponents>();
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
            if (PlayerState.isNormal)
            {
                FollowPlayer(playerComponents.normalGameObject);
            }
            else if (PlayerState.isGhost)
            {
                FollowPlayer(playerComponents.ghostGameObject);
            }
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

        if (targetPlayer.transform.position.x - transform.position.x < 0)
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

        dynamicSpeed = distance - PLAYER_TO_GLASYA_DISTANCE;
        
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, targetPosition, dynamicSpeed * Time.deltaTime);
    }
}
