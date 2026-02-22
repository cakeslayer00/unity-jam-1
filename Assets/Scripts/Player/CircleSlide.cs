using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CircleSlide : MonoBehaviour
{
    [Header("Circle source")]
    [SerializeField] private CircleCollider2D circle;

    [Header("Cube size (for staying inside)")]
    [SerializeField] private float extraInset; 

    [Header("Movement")]
    [SerializeField] private float angularSpeedDegree = 180f;

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    
    private float _angleDegree;
    private int _direction = 1;
    private bool _flipRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    { 
        Vector2 center = GetCenterWorld();
        Vector2 fromCenter = _rb.position - center;
        if (fromCenter.sqrMagnitude < 0.0001f) fromCenter = Vector2.right;

        _angleDegree = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;

        // Snap to correct spot + rotate
        ApplyPositionAndRotation();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _flipRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (_flipRequested)
        {
            _direction *= -1;
            _flipRequested = false;
        }

        _angleDegree += _direction * angularSpeedDegree * Time.fixedDeltaTime;
        ApplyPositionAndRotation();
    }

    private void ApplyPositionAndRotation()
    {
        Vector2 center = GetCenterWorld();
        float r = GetEffectiveRadiusWorld(); // circle radius minus cube half-height

        float rad = _angleDegree * Mathf.Deg2Rad;
        Vector2 outward = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        Vector2 pos = center + outward * r;
        _rb.MovePosition(pos);

        // Make cube "perpendicular to circle": cube's UP points outward (normal)
        // For that, z-rotation should be (angleDeg - 90)
        _rb.MoveRotation(_angleDegree - 90f);
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
        float worldHeight = _collider.size.y * Mathf.Abs(s.y);
        return worldHeight * 0.5f;
    }

    private float GetEffectiveRadiusWorld()
    {
        float r = GetCircleRadiusWorld() - GetCubeHalfHeightWorld() - extraInset;
        return Mathf.Max(0f, r);
    }
}