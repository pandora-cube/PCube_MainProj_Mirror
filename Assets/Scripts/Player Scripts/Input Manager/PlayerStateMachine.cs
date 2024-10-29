using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    public bool isGhost;
    public bool isNormal;
    public bool canMove;
    public bool isCrawling;
    public bool isOnSlope;
    public bool canDash;
    public bool isGrounded;

    public static PlayerStateMachine instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        isNormal = true;
        isGhost = false;
        canMove = true;
        isCrawling = false;
        isOnSlope = false;
        canDash = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isNormal = true;
        isGhost = false;
        canMove = true;
        isCrawling = false;
        isOnSlope = false;
        canDash = true;
    }
}
