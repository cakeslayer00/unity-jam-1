using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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
        
        (0f, 10f, 0.33f),   // Было 0.4 -> стало 0.3 (дистанция сократится)
        (10f, 14f, 0.3f), // Было 0.35 -> стало 0.25
        (24f, 18f, 0.2f),  // Было 0.3 -> стало 0.2
        
    };

    [Header("Timed Event")]
    [SerializeField] private float eventTime = 13f;
    [SerializeField] private UnityEvent onEventTime;

    private bool eventFired;

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

        // fires once when time crosses 13s
        if (!eventFired && Time.timeSinceLevelLoad >= eventTime)
        {
            eventFired = true;
            onEventTime?.Invoke();
        }
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

    // Измените начальное значение здесь
    private bool spawnOnTop = false; 

    void SpawnPlatform()
    {
        // При первом вызове false превратится в true -> первая платформа ВЕРХНЯЯ
        spawnOnTop = !spawnOnTop;

        float y;
        if (spawnOnTop)
        {
            // Рандом вокруг верхней точки
            y = Random.Range(highY - 1.2f, highY + 1.2f); 
        }
        else
        {
            // Рандом вокруг нижней точки
            y = Random.Range(lowY - 1.2f, lowY + 1.2f);
        }

        Vector3 spawnPos = new Vector3(spawnX, y, 0);
        Instantiate(platformPrefab, spawnPos, Quaternion.identity);
    }
}