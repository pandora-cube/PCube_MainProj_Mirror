using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public const int PLAYER_LAYER = 3;
    protected float playerDetectionRadius;
    protected float attackDamage;
    protected float attackDelay;
    public abstract void DetectPlayer();
    public abstract void AttackPlayer(Collider2D playerCollider);
}
