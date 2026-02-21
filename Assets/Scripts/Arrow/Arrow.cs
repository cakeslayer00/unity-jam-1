using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}