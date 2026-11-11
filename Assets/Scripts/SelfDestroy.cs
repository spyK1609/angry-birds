using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float health;
    private DynamiteScript dynamite;
    private Rigidbody2D rigidbody;
    public static float sensitivity = 0.5f;
    private GameManager gameManager;
    private bool isDynamite;
    private bool isExploded = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        dynamite = GetComponent<DynamiteScript>();
        isDynamite = dynamite != null;
    }

    public void DestroyObject(GameObject obj)
    {
        if (isExploded) return;
        if (isDynamite)
        {
            dynamite.Explode();
            isExploded = true;
            return;
        }
        Destroy(obj);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody.linearVelocity.magnitude > 3f)
        {
            health -= rigidbody.linearVelocity.magnitude * rigidbody.mass * sensitivity;
        }
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.linearVelocity.magnitude > 1f)
            {
                health -= rb.linearVelocity.magnitude;
            }
        }
        if (health <= 0)
        {
            if (gameObject.tag == "Pig")
            {
                gameManager.ChangePigsCount();
            }
            if (collision.gameObject.tag == "Player")
            {
                ParticleSystem ps = collision.gameObject.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
            }
            DestroyObject(gameObject);
        }


    }
}
