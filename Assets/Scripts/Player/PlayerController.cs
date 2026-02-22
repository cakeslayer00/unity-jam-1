using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[Obsolete("This script is deprecated. Please use the new PlayerController2D instead.")]
public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float maxHeight = 3f;
    [SerializeField] private float fallGravity = 4f;
    [SerializeField] private float holdGravity = 0.5f;

    [Header("Health")]
    [SerializeField] private int maxHits = 3;
    [SerializeField] private float invincibleSeconds = 3f;
    [SerializeField] private float flickerInterval = 0.1f;

    private int hitsTaken = 0;
    private bool invincible = false;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float groundY;

    private SpriteRenderer[] renderers;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>(true);

        if (renderers.Length == 0)
            Debug.LogWarning("No SpriteRenderer found on Player or its children. Flicker won't show.");
        col = GetComponent<Collider2D>();

        groundY = transform.position.y;

        if (col == null)
            Debug.LogWarning("Player has no Collider2D.");
    }

    void Update()
    {
        bool spaceHeld = Keyboard.current.spaceKey.isPressed;
        bool spacePressed = Keyboard.current.spaceKey.wasPressedThisFrame;

        if (spacePressed)
        {
            rb.linearVelocity = new Vector2(0, jumpSpeed);
            rb.gravityScale = holdGravity;
            isGrounded = false;
        }

        if (spaceHeld && !isGrounded) rb.gravityScale = holdGravity;
        if (!spaceHeld && !isGrounded) rb.gravityScale = fallGravity;

        if (!isGrounded && transform.position.y >= groundY + maxHeight)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");

        if (!other.CompareTag("Arrow")) return;
        if (invincible) return;

        TakeHit();
        Destroy(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.gravityScale = 1f;
            groundY = transform.position.y;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void TakeHit()
    {
        Debug.Log("HIT!");
        if (invincible) return;

        hitsTaken++;
        Debug.Log($"Player hit! {hitsTaken}/{maxHits}");

        if (hitsTaken >= maxHits)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        invincible = true;

        float t = 0f;
        bool on = false;

        while (t < invincibleSeconds)
        {
            on = !on;
            foreach (var r in renderers)
                r.enabled = on;

            yield return new WaitForSeconds(flickerInterval);
            t += flickerInterval;
        }

        foreach (var r in renderers)
            r.enabled = true;

        invincible = false;
    }

    public void Die()
    {
        Debug.Log("Player Died");

        foreach (var r in renderers)
            r.enabled = false;

        if (col != null) col.enabled = false;
        enabled = false;
    }
}