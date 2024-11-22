using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parryable : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    private bool isInvincible = false;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        boxCollider2D = GetComponentInParent<BoxCollider2D>();
    }

    public void Parry()
    {
        transform.parent.tag = "Invincible";

        StartCoroutine(ToogleBoxCollider2D());
    }

    private IEnumerator ToogleBoxCollider2D()
    {

        yield return new WaitForSeconds(3);

        transform.parent.tag = "Enemy";
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
