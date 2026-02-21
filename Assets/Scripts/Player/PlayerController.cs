using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float fallGravity = 4f;    // мягче падение
    [SerializeField] private float holdGravity = 0.5f;  // лёгкое притяжение пока зажат пробел

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float groundY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundY = transform.position.y;
    }

    void Update()
    {
        bool spaceHeld = Keyboard.current.spaceKey.isPressed;
        bool spacePressed = Keyboard.current.spaceKey.wasPressedThisFrame;

        // прыжок с земли или повторный прыжок в воздухе
        if (spacePressed)
        {
            rb.linearVelocity = new Vector2(0, jumpSpeed);
            rb.gravityScale = holdGravity;
        }

        // пока держим пробел — лёгкая гравитация (плавный подъём)
        if (spaceHeld && !isGrounded)
        {
            rb.gravityScale = holdGravity;
        }

        // отпустили пробел — мягко падаем
        if (!spaceHeld && !isGrounded)
        {
            rb.gravityScale = fallGravity;
        }

        // ограничение высоты
        if (!isGrounded && transform.position.y >= groundY + maxHeight)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.gravityScale = 1f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.gravityScale = 1f;
        }

        if (collision.gameObject.CompareTag("Arrow"))
        {
            Die();
        }
    }

    public void Die()
{
    Debug.Log("Player Died");

    // Option 1: Destroy player
    // Destroy(gameObject);

    // Option 2: Reload scene
    // UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    // Option 3: Disable controls
    enabled = false;
}
}