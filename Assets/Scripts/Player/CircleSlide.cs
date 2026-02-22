using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CircleSlide : MonoBehaviour
{
    [Header("Circle source")]
    [SerializeField] private string circleTag = "Circle";
    [SerializeField] private CircleCollider2D circle; // optional manual assign

    [Header("Cube size (for staying inside)")]
    [SerializeField] private BoxCollider2D box;        // optional manual assign
    [SerializeField] private float extraInset = 0.00f; // small safety margin

    [Header("Movement")]
    [SerializeField] private float angularSpeedDeg = 180f;
    [SerializeField] private KeyCode flipKey = KeyCode.Space;

    private Rigidbody2D rb;

    private float angleDeg;
    private int direction = 1;
    private bool flipRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (!circle)
        {
            var go = GameObject.FindGameObjectWithTag(circleTag);
            if (go) circle = go.GetComponent<CircleCollider2D>();
        }

        if (!box) box = GetComponent<BoxCollider2D>();

        if (!circle)
        {
            Debug.LogError($"No CircleCollider2D found. Tag your circle object as '{circleTag}' or assign it.");
            enabled = false;
            return;
        }

        if (!box)
        {
            Debug.LogError("No BoxCollider2D found on the player. Add one or assign it.");
            enabled = false;
            return;
        }

        // Initialize angle from spawn position
        Vector2 center = GetCenterWorld();
        Vector2 fromCenter = rb.position - center;
        if (fromCenter.sqrMagnitude < 0.0001f) fromCenter = Vector2.right;

        angleDeg = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;

        // Snap to correct spot + rotate
        ApplyPositionAndRotation();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            flipRequested = true;
    }

    private void FixedUpdate()
    {
        if (flipRequested)
        {
            direction *= -1;
            flipRequested = false;
        }

        angleDeg += direction * angularSpeedDeg * Time.fixedDeltaTime;
        ApplyPositionAndRotation();
    }

    private void ApplyPositionAndRotation()
    {
        Vector2 center = GetCenterWorld();
        float r = GetEffectiveRadiusWorld(); // circle radius minus cube half-height

        float rad = angleDeg * Mathf.Deg2Rad;
        Vector2 outward = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        Vector2 pos = center + outward * r;
        rb.MovePosition(pos);

        // Make cube "perpendicular to circle": cube's UP points outward (normal)
        // For that, z-rotation should be (angleDeg - 90)
        rb.MoveRotation(angleDeg - 90f);
    }

    private Vector2 GetCenterWorld()
    {
        Transform t = circle.transform;
        // circle.offset is local space; scale it and add to world position
        Vector2 scaledOffset = Vector2.Scale(circle.offset, t.lossyScale);
        return (Vector2)t.position + scaledOffset;
    }

    private float GetCircleRadiusWorld()
    {
        Vector3 s = circle.transform.lossyScale;
        float scale = Mathf.Max(Mathf.Abs(s.x), Mathf.Abs(s.y));
        return circle.radius * scale;
    }

    private float GetCubeHalfHeightWorld()
    {
        // Since we align cube UP to the radius, cube "height" (local Y) is the radial thickness.
        Vector3 s = transform.lossyScale;
        float worldHeight = box.size.y * Mathf.Abs(s.y);
        return worldHeight * 0.5f;
    }

    private float GetEffectiveRadiusWorld()
    {
        float r = GetCircleRadiusWorld() - GetCubeHalfHeightWorld() - extraInset;
        return Mathf.Max(0f, r);
    }
}