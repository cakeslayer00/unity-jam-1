using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerHitBlink : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHits = 3;
    [SerializeField] private float invincibleSeconds = 3f;
    [SerializeField] private float flickerInterval = 0.1f;
    

    private int hitsTaken = 0;
    private bool invincible = false;

    private SpriteRenderer[] renderers;
    private Collider2D col;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // If collider is on a child, root tag check makes it robust
        bool isArrow = other.CompareTag("Arrow") || other.transform.root.CompareTag("Arrow");
        if (!isArrow) return;

        if (invincible) return;

        TakeHit();

        // Destroy whole arrow prefab (root), not just child collider
        Destroy(other.transform.root.gameObject);
    }

    private void TakeHit()
    {
        Debug.Log("HIT!");
        hitsTaken++;

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
        bool lowAlpha = false;

        while (t < invincibleSeconds)
        {
            lowAlpha = !lowAlpha;
            SetAlpha(lowAlpha ? 0.4f : 1f);

            yield return new WaitForSeconds(flickerInterval);
            t += flickerInterval;
        }

        // Restore full opacity
        SetAlpha(1f);
        invincible = false;
    }

    private void Die()
    {
        Debug.Log("Player Died");

        foreach (var r in renderers) r.enabled = false;
        if (col != null) col.enabled = false;

        enabled = false;
    }

    private void SetAlpha(float alpha)
    {
        foreach (var r in renderers)
        {
            Color c = r.color;
            c.a = alpha;
            r.color = c;
        }
    }
}