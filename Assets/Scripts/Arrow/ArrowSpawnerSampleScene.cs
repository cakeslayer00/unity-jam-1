using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerSampleScene : MonoBehaviour
{
[Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float bpm = 120f;
    [SerializeField] private float startDelay = 0.0f; // seconds if you need alignment

    [Header("Session")]
    [SerializeField] private float duration = 30f;

    [Header("Difficulty curves (0..1 -> value)")]
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 6, 1, 16);
    // rateCurve returns how many "subdivisions per beat" to use (1=quarter, 2=eighth, 4=sixteenth)
    [SerializeField] private AnimationCurve rateCurve = AnimationCurve.EaseInOut(0, 1, 1, 4);
    [SerializeField] private AnimationCurve spawnChance = AnimationCurve.EaseInOut(0, 0.35f, 1, 0.85f);

    [Header("Spawning")]
    [SerializeField] private ArrowSampleScene arrowPrefab;
    [SerializeField] private float spawnX = 9f;
    [SerializeField] private float topY = 4.5f;
    [SerializeField] private float bottomY = -4.5f;
    [SerializeField] private int laneCount = 10;

    [Header("Groups")]
    [SerializeField] private float burstChance = 0.25f;     // chance to spawn 2-4 on a beat
    [SerializeField] private float miniWaveChance = 0.15f;  // chance to spawn short adjacent block

    private double dspStart;
    private double nextTickDsp;
    private float elapsed;

    private void Start()
    {
        dspStart = AudioSettings.dspTime + startDelay;

        // Start song exactly on dspStart for tight sync
        audioSource.PlayScheduled(dspStart);

        nextTickDsp = dspStart;
    }

    private void Update()
    {
        // progress 0..1 across duration
        elapsed = (float)(AudioSettings.dspTime - dspStart);
        float p = Mathf.Clamp01(elapsed / duration);
        if (p >= 1f) return; // stop after 30s (or keep going if you want)

        // Decide current subdivision from curve
        float sub = Mathf.Clamp(rateCurve.Evaluate(p), 1f, 8f);
        int subdivision = QuantizeSubdivision(sub); // 1,2,4,8

        double secondsPerBeat = 60.0 / bpm;
        double tick = secondsPerBeat / subdivision;

        // Spawn as many ticks as we missed (keeps up even if FPS drops)
        double now = AudioSettings.dspTime;
        while (now >= nextTickDsp)
        {
            SpawnOnTick(p);
            nextTickDsp += tick;
        }
    }

    private int QuantizeSubdivision(float v)
    {
        // maps to nearest of 1,2,4,8
        if (v < 1.5f) return 1;
        if (v < 3f) return 2;
        if (v < 6f) return 4;
        return 8;
    }

    private void SpawnOnTick(float progress01)
    {
        if (Random.value > spawnChance.Evaluate(progress01))
            return; // skip this tick
        
        float speed = speedCurve.Evaluate(progress01);

        // As progress increases, increase group chances a bit
        float burstP = Mathf.Lerp(burstChance * 0.6f, burstChance, progress01);
        float waveP  = Mathf.Lerp(miniWaveChance * 0.5f, miniWaveChance, progress01);

        float r = Random.value;

        if (r < waveP)
        {
            SpawnMiniWave(Random.Range(3, Mathf.Min(7, laneCount)), speed);
        }
        else if (r < waveP + burstP)
        {
            SpawnBurst(Random.Range(2, 5), speed);
        }
        else
        {
            SpawnSingle(speed);
        }
    }

    private void SpawnSingle(float speed)
    {
        SpawnAtLane(Random.Range(0, laneCount), speed);
    }

    private void SpawnBurst(int count, float speed)
    {
        for (int i = 0; i < count; i++)
            SpawnAtLane(Random.Range(0, laneCount), speed);
    }

    private void SpawnMiniWave(int length, float speed)
    {
        int maxLength = Mathf.Max(1, laneCount - 2); // keep 2 lanes free
        length = Mathf.Clamp(length, 1, maxLength);
        int startLane = Random.Range(0, laneCount - length + 1);
        for (int lane = startLane; lane < startLane + length; lane++)
            SpawnAtLane(lane, speed);
    }

    private void SpawnAtLane(int lane, float speed)
    {
        float y = LaneToY(lane);
        var arrow = Instantiate(arrowPrefab, new Vector3(spawnX, y, 0f), Quaternion.identity);
        arrow.SetSpeed(speed);
    }

    private float LaneToY(int lane)
    {
        if (laneCount <= 1) return (topY + bottomY) * 0.5f;
        float t = lane / (laneCount - 1f);
        return Mathf.Lerp(bottomY, topY, t);
    }
}
