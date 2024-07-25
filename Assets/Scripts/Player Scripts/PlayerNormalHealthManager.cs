using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalHealthManager : MonoBehaviour, IDamageable
{
    public float maxHealth { get ; set ; }
    public float currentHealth { get ; set; }

    [Header("Collision Detection")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 1f;
        currentHealth = maxHealth;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D called");
        Debug.Log("collided layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("DAMAGE!!");
            TakeDamage(1f);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
