using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Slide : MonoBehaviour
{
[Header("Wall follow")]
    [SerializeField] private Transform wall;
    [SerializeField] private float xOffsetFromWall = 0.8f;   // distance from wall

    [Header("Player movement")]
    [SerializeField] private float speedY = 10f;
    [SerializeField] private float topY = 4f;                // WORLD bounds
    [SerializeField] private float bottomY = -4f;

    private Rigidbody2D rb;
    private float direction = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            direction *= -1f;
    }

    private void FixedUpdate()
    {
        if (!wall) return;

        float targetX = wall.position.x + xOffsetFromWall;

        float newY = rb.position.y + direction * speedY * Time.fixedDeltaTime;
        newY = Mathf.Clamp(newY, bottomY, topY);

        rb.MovePosition(new Vector2(targetX, newY));
    }
}