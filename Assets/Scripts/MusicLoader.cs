using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        // If another MusicPlayer already exists, kill this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // This is the one true instance
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}