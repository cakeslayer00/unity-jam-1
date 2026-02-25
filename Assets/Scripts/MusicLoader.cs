using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    public static MusicLoader Instance;
    public AudioSource source;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!source) source = GetComponent<AudioSource>();
        if (!source.isPlaying) source.Play();
    }
}