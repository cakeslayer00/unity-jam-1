using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Track track;
    public AudioSource musicSource;

    [Header("Positions")]
    public float spawnX = 10f;
    public float targetX = 0f;
    public float spawnY = 0.4f;

    [Header("Arrow")]
    public float arrowSpeed = 5f;

    private int beatIndex = 0;
    private float travelTime;

    void Start()
    {
        // How long arrow needs to reach the player
        float distance = spawnX - targetX;
        travelTime = distance / arrowSpeed;

        // Start music here (important!)
        musicSource.Play();
    }

    void Update()
    {
        if (beatIndex >= track.beatTimings.Count)
            return;

        float songTime = musicSource.time;
        float spawnTime = track.beatTimings[beatIndex] - travelTime;

        if (songTime >= spawnTime)
        {
            SpawnArrow();
            beatIndex++;
        }
    }

    void SpawnArrow()
    {
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
        Instantiate(arrowPrefab, spawnPosition, rotation);
    }
}