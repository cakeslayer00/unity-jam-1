using UnityEngine;

public class BoundaryMove : MonoBehaviour
{
    [SerializeField] private float duration = 30f;

    // Default curve is linear; you’ll edit it in Inspector
    [SerializeField] private AnimationCurve ease = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // If you only want horizontal movement, enable this
    [SerializeField] private bool onlyX = true;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float t;

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

        float u = Mathf.Clamp01(t / duration);   // 0..1 (time)
        float easedU = ease.Evaluate(u);         // 0..1 (your curve)

        transform.position = Vector3.LerpUnclamped(startPos, targetPos, easedU);
    }
}
