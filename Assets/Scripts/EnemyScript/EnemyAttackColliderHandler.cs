using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    EnemyAttackManager enemyAttackManager;

    void Awake()
    {
        enemyAttackManager = GetComponentInParent<EnemyAttackManager>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        enemyAttackManager.HandleAttackCollision(collider2D);
    }
}
