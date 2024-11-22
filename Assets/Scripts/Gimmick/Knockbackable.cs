using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbackable : MonoBehaviour
{
    [Header("Knockback Variables")]
    [SerializeField] private float knockbackForce;
    [SerializeField] private bool isPlayer;
    [SerializeField] private float knocbackFallOffDuration;
    [SerializeField] private PhysicsMaterial2D fullFrictionMaterial;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Transform knockbackSource)
    {
        // base knockbackDirection
        Vector2 knockbackDirection = (transform.position - knockbackSource.position).normalized;

        //knockback only in  x
        knockbackDirection.y = 0f;

        // real final vector
        Vector2 knockbackVector = knockbackDirection * knockbackForce;

        if (isPlayer)
        {
            if (PlayerStateMachine.instance.isGhost)
            {
                PlayerComponents.instance.ghostRb.velocity = Vector2.zero;
                PlayerComponents.instance.ghostRb.AddForce(knockbackVector, ForceMode2D.Impulse);
                StartCoroutine(GradualVelocityFallOff(PlayerComponents.instance.ghostRb));
            }
        }
        else
        {
            if (rb != null)
            {
                rb.sharedMaterial = null;
                rb.velocity = Vector2.zero;
                rb.AddForce(knockbackVector, ForceMode2D.Impulse);
                StartCoroutine(GradualVelocityFallOff(rb));
            }
        }
    }


    //slowly decrease the rb velocity
    IEnumerator GradualVelocityFallOff(Rigidbody2D rb)
    {
        float elapsedTime = 0f;

        Vector2 initalVelocity = rb.velocity;

        while (elapsedTime < knocbackFallOffDuration)
        {
            elapsedTime += Time.deltaTime;
            rb.velocity = Vector2.Lerp(initalVelocity, Vector2.zero, elapsedTime / knocbackFallOffDuration);

            yield return null;
        }

        rb.velocity = Vector2.zero;
        rb.sharedMaterial = fullFrictionMaterial;
    }
}