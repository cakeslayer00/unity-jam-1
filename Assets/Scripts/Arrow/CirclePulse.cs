using UnityEngine;

public class CirclePulse : MonoBehaviour
{
    public AudioSource audioSource;
    public int fftSize = 1024;
    public FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    public float lowHz = 60f;
    public float highHz = 150f;

    [Range(0.001f, 0.2f)] public float avgFollow = 0.02f;
    public float thresholdMultiplier = 1.5f;
    public float minBeatInterval = 0.12f;

    float[] _spectrum;
    float _avgEnergy = 0.0001f;
    float _pulse;
    float _lastBeatTime;

    public float Pulse => _pulse; // <--- ADD THIS

    void Awake()
    {
        _spectrum = new float[Mathf.NextPowerOfTwo(fftSize)];
        if (audioSource == null) audioSource = FindFirstObjectByType<AudioSource>();
    }

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            _pulse = Mathf.MoveTowards(_pulse, 0f, 8f * Time.deltaTime);
            return;
        }

        audioSource.GetSpectrumData(_spectrum, 0, fftWindow);

        float energy = BandEnergy(lowHz, highHz);
        _avgEnergy = Mathf.Lerp(_avgEnergy, energy, avgFollow);

        bool isBeat = false;
        if (Time.time - _lastBeatTime >= minBeatInterval &&
            energy > _avgEnergy * thresholdMultiplier)
        {
            isBeat = true;
            _lastBeatTime = Time.time;
        }

        float target = isBeat ? 1f : 0f;
        float speed = isBeat ? 25f : 8f;
        _pulse = Mathf.MoveTowards(_pulse, target, speed * Time.deltaTime);
    }

    float BandEnergy(float low, float high)
    {
        float nyquist = AudioSettings.outputSampleRate * 0.5f;
        int n = _spectrum.Length;

        int lowBin = Mathf.Clamp(Mathf.FloorToInt((low / nyquist) * n), 0, n - 1);
        int highBin = Mathf.Clamp(Mathf.CeilToInt((high / nyquist) * n), 0, n - 1);
        if (highBin < lowBin) (lowBin, highBin) = (highBin, lowBin);

        float sum = 0f;
        for (int i = lowBin; i <= highBin; i++) sum += _spectrum[i];
        return sum / Mathf.Max(1, (highBin - lowBin + 1));
    }
}