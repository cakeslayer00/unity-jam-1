using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * PlatformSpawner.CurrentSpeed * Time.deltaTime);

        if (transform.position.x < -20f)
            Destroy(gameObject);
    }
}