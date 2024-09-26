using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.CompareTag("CameraCollider")) 
        {
            Destroy(gameObject);
        }
    }
}
