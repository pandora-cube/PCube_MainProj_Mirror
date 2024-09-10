using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    const int ATTACKABLE_LAYER = 12;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ATTACKABLE_LAYER)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(1);
            }
        }
    }
}
