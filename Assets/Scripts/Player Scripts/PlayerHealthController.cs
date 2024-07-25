using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    public float maxHealth { get ; set ; }
    public float currentHealth { get ; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        //game over...
    }
}
