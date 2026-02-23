using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float spawnX = 10f;
    [SerializeField] private float lowY = -2f;
    [SerializeField] private float highY = 2f;
    [SerializeField] private float speedLerpRate = 2f;

    public static float CurrentSpeed = 6f;
    private float targetSpeed = 6f;
    private float currentInterval = 0.3f;

    private (float time, float speed, float interval)[] speedMap = new (float, float, float)[]
    {
        (0f, 6f, 0.35f),
        (10f, 9f, 0.35f),
        (24f, 13f, 0.3f),
        (39f, 6f, 0.35f),
        (47f, 10f, 0.3f),
        (80f, 13f, 0.3f)
    };

    private bool spawnLow = true;

    void Start()
    {
        CurrentSpeed = 6f;
        targetSpeed = 6f;
        StartCoroutine(SpawnLoop());
        StartCoroutine(SpeedController());
    }

    void Update()
    {
        CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, targetSpeed, speedLerpRate * Time.deltaTime);
    }

    IEnumerator SpeedController()
    {
        float startTime = Time.time;
        foreach (var s in speedMap)
        {
            float waitTime = s.time - (Time.time - startTime);
            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);
            targetSpeed = s.speed;
            currentInterval = s.interval;
        }
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnPlatform();

        while (true)
        {
            yield return new WaitForSeconds(currentInterval);
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        spawnLow = !spawnLow;
        float y = spawnLow ? lowY : highY;
        Vector3 spawnPos = new Vector3(spawnX, y, 0);
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);
    }
}