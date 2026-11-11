using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamiteScript : MonoBehaviour
{
    public float radius_ = 5f;
    public float force = 10f;
    public float upwardsModifier = 0.5f;
    private Rigidbody2D rb;
    // public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Explode()
    {
        StartCoroutine(ExplodeEffect());
    }

    private IEnumerator ExplodeEffect()
    {
        rb.bodyType = RigidbodyType2D.Static;
        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = 0.5f;
        circleCollider.offset = Vector3.zero;
        circleCollider.isTrigger = false;
        for (int i = 0; circleCollider.radius < radius_; i++)
        {
            circleCollider.radius += 0.25f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null || rb.isKinematic) return;
        rb.AddForce(force * rb.linearVelocity, ForceMode2D.Impulse);
    }
}
