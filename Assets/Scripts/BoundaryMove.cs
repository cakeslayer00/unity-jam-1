using UnityEngine;
using UnityEngine.Events;

public class BoundaryMove : MonoBehaviour
{
    [SerializeField] private float duration = 30f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private bool onlyX = true;

    [Header("Event timing (0..1)")]
    [Range(0f, 1f)]
    [SerializeField] private float fireAt = 0.9f;   // 90% by default
    [SerializeField] private UnityEvent onNearEnd;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float t;

    private bool fired;

    void Start()
    {
        startPos = transform.position;

        var cam = Camera.main;
        Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        center.z = startPos.z;

        targetPos = center;
        if (onlyX) targetPos.y = startPos.y;
    }

    void Update()
    {
        t += Time.deltaTime;

        float u = Mathf.Clamp01(t / duration);

        // fire once when we cross the threshold
        if (!fired && u >= fireAt)
        {
            fired = true;
            onNearEnd?.Invoke();
        }

        float easedU = ease.Evaluate(u);
        transform.position = Vector3.LerpUnclamped(startPos, targetPos, easedU);
    }
}