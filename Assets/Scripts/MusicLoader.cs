using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    public static MusicLoader Instance { get; private set; }

    [SerializeField] private AudioSource source;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!source) source = GetComponent<AudioSource>();

        if (!source.isPlaying)
            source.Play();
    }

    public void RestartFromBeginning()
    {
        if (!source) return;

        source.Stop();
        source.time = 0f;   // reset timeline
        source.Play();
    }

    public AudioSource Source => source;
}