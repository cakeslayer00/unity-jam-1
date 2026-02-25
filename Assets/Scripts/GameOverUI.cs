using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Scene 1 build index")]
    [SerializeField] private int scene1BuildIndex = 0;

    private bool shown;

    private void Awake()
    {
        Time.timeScale = 1f;
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (health) health.OnDied += Show;
    }

    private void OnDisable()
    {
        if (health) health.OnDied -= Show;
    }

    private void Show()
    {
        if (shown) return;
        shown = true;

        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // freeze gameplay

        // Optional: pause music while on game over screen
        if (MusicLoader.Instance && MusicLoader.Instance.Source)
            MusicLoader.Instance.Source.Pause();
    }

    // Hook this to your Restart button
    public void RestartToScene1()
    {
        Time.timeScale = 1f;

        // Restart music from 0
        if (MusicLoader.Instance)
            MusicLoader.Instance.RestartFromBeginning();

        // Load Scene 1
        SceneManager.LoadScene(scene1BuildIndex);
    }
}