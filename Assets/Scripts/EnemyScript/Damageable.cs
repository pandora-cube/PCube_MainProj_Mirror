using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public const int PLAYER_LAYER = 3;
    public int maxHealth;
    public int currentHealth;
    public abstract void DetectPlayer();
    public abstract IEnumerator AttackPlayer(Collider2D playerCollider);
    public abstract void TakeDamage(int damageAmount);
    public abstract void Die();
}
