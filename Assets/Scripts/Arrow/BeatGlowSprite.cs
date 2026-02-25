using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BeatGlowSprite : MonoBehaviour
{
    public CirclePulse beat;

    [Header("Scale pulse")]
    public float baseScale = 1f;
    public float scaleAdd = 0.12f;

    [Header("Glow pulse (HDR)")]
    public Color baseColor = Color.white;
    public float baseGlow = 1f;      // 1 = normal brightness
    public float glowAdd = 2.5f;     // how much extra HDR on beat

    SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        if (beat == null) beat = FindFirstObjectByType<CirclePulse>();
    }

    void Update()
    {
        float p = beat != null ? beat.Pulse : 0f;

        // scale
        float s = baseScale + p * scaleAdd;
        transform.localScale = Vector3.one * s;

        // HDR-ish glow (push above 1 so Bloom reacts)
        float glow = baseGlow + p * glowAdd;
        _sr.color = baseColor * glow;
    }
}