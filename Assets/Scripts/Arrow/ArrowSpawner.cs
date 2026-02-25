using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Track track;
    public AudioSource musicSource;

    [Header("Spawn Center")]
    public Vector2 spawnCenter = Vector2.zero;

    [Header("Direction Rules")]
    [SerializeField] private float minAngleSeparation = 90f; // degrees
    [SerializeField] private int maxTries = 30;

    private int beatIndex = 0;
    private float lastAngle = float.NaN;

    void Start()
    {
        musicSource.Play();
    }

    void Update()
    {
        if (!track || !musicSource) return;
        if (beatIndex >= track.beatTimings.Count) return;

        float songTime = musicSource.time;
        float beatTime = track.beatTimings[beatIndex];

        if (songTime >= beatTime)
        {
            SpawnArrow();
            beatIndex++;
        }
    }

    void SpawnArrow()
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
        arrow.Init(dir);
    }

    float PickAngleNotNearPrevious()
    {
        // first arrow: anything
        if (float.IsNaN(lastAngle))
            return Random.Range(0f, 360f);

        for (int i = 0; i < maxTries; i++)
        {
            float candidate = Random.Range(0f, 360f);

            // smallest difference on a circle
            float diff = Mathf.Abs(Mathf.DeltaAngle(candidate, lastAngle));

            if (diff >= minAngleSeparation)
                return candidate;
        }

        // Fallback if RNG is unlucky: force exactly min separation away
        return Mathf.Repeat(lastAngle + minAngleSeparation, 360f);
    }
}