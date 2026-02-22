using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Track track;
    public AudioSource musicSource;

    [Header("Spawn Center")]
    public Vector2 spawnCenter = Vector2.zero; // set (0,0) or drag a Transform version below

    private int beatIndex = 0;

    void Start()
    {
        musicSource.Play();
    }

    void Update()
    {
        if (track == null || musicSource == null) return;
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
        Vector2 spawnPosition = Vector2.zero; // center of circle

        GameObject arrowObj = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // Random direction (full circle)
        float angle = Random.Range(0f, 360f);
        Vector2 dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrow.Init(dir);
    }
}