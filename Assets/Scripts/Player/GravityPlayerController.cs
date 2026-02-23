using UnityEngine;
using UnityEngine.InputSystem;

public class GravityPlayerController : MonoBehaviour
{
    [SerializeField] private float gravityStrength = 5f;
    [SerializeField] private float rayLength = 0.6f;

    private Rigidbody2D rb;
    private bool gravityDown = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityStrength;
    }

    void Update()
    {
        bool isOnPlatform = IsOnPlatform();

        if (Mouse.current.leftButton.wasPressedThisFrame && isOnPlatform)
        {
            gravityDown = !gravityDown;
            rb.gravityScale = gravityDown ? gravityStrength : -gravityStrength;
            rb.linearVelocity = Vector2.zero;
        }
    }

    bool IsOnPlatform()
    {
        Vector2 direction = gravityDown ? Vector2.down : Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Platform"));
        return hit.collider != null;
    }
}