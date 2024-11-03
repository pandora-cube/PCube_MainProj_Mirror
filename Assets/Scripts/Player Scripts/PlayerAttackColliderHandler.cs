using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackColliderHandler : MonoBehaviour
{
    private PlayerAttackManager playerAttackManager;

    void Awake()
    {
        playerAttackManager = GetComponentInParent<PlayerAttackManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        playerAttackManager.HandleAttackCollision(collider2D);
    }
}
