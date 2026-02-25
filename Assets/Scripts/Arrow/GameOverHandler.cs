using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;

    private void OnEnable()
    {
        if (health) health.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (health) health.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        Time.timeScale = 0f; // freeze game
        Debug.Log("Game Over");
        // later: show game over panel / restart button
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}