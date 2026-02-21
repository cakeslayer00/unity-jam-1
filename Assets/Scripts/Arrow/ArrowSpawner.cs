using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnInterval = 2f;
    public float spawnY = 0.4f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnArrow();
            timer = 0f;
        }
    }

    void SpawnArrow()
    {
        Vector2 spawnPosition = new Vector2(10f, spawnY);
        // Rotate 90 degrees around Z-axis
        Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
        Instantiate(arrowPrefab, spawnPosition, rotation);
    }
}