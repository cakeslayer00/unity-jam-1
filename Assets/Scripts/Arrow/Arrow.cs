using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 direction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by spawner
    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate arrow to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        // ↑ -90 because your triangle sprite points UP by default
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}