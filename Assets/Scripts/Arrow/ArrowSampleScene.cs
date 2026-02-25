using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowSampleScene : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Face LEFT (flip). If already left, remove.
        var s = transform.localScale;
        s.x = -Mathf.Abs(s.x);
        transform.localScale = s;
    }

    public void SetSpeed(float newSpeed) => speed = newSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeHit(1);

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}