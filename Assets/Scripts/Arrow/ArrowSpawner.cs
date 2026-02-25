using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Track track;

    [Header("Music")]
    public AudioSource musicSource; // will auto-find if null
    [Tooltip("Tag on the persistent music GameObject (DontDestroyOnLoad).")]
    public string musicTag = "Music";

    [Header("Level timing")]
    [Tooltip("Song time (seconds) when this scene should start spawning.")]
    public float spawnStartTime = 0f;

    [Header("Spawn Center")]
    public Vector2 spawnCenter = Vector2.zero;

    [Header("Direction Rules")]
    [SerializeField] private float minAngleSeparation = 90f; // degrees
    [SerializeField] private int maxTries = 30;

    private int beatIndex = 0;
    private float lastAngle = float.NaN;
    private bool initialized = false;

    private void Awake()
    {
        // Find the persistent AudioSource at runtime
        if (!musicSource)
        {
            var musicObj = GameObject.FindWithTag(musicTag);
            if (musicObj) musicSource = musicObj.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!track || !musicSource) return;
        if (!musicSource.isPlaying) return;

        float songTime = musicSource.time;

        // Wait until this level's section starts
        if (songTime < spawnStartTime) return;

        // First frame after we reach spawnStartTime: jump to correct beat index
        if (!initialized)
        {
            beatIndex = FindFirstBeatIndexAtOrAfter(spawnStartTime);
            initialized = true;
        }

        // Spawn due beats (handles FPS drops)
        while (beatIndex < track.beatTimings.Count && songTime >= track.beatTimings[beatIndex])
        {
            SpawnArrow();
            beatIndex++;
        }
    }

    private int FindFirstBeatIndexAtOrAfter(float t)
    {
        for (int i = 0; i < track.beatTimings.Count; i++)
            if (track.beatTimings[i] >= t)
                return i;

        return track.beatTimings.Count;
    }

    private void SpawnArrow()
    {
        Vector2 spawnPosition = spawnCenter;
        GameObject arrowObj = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        float angle = PickAngleNotNearPrevious();
        lastAngle = angle;

        Vector2 dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        if (arrow) arrow.Init(dir);
    }

    private float PickAngleNotNearPrevious()
    {
        if (float.IsNaN(lastAngle))
            return Random.Range(0f, 360f);

        for (int i = 0; i < maxTries; i++)
        {
            float candidate = Random.Range(0f, 360f);
            float diff = Mathf.Abs(Mathf.DeltaAngle(candidate, lastAngle));
            if (diff >= minAngleSeparation)
                return candidate;
        }

        return Mathf.Repeat(lastAngle + minAngleSeparation, 360f);
    }
}