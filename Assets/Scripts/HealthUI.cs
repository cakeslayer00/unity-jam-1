using UnityEngine;

using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (!text) text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (!playerHealth) return;
        playerHealth.OnLivesChanged += UpdateText;
        playerHealth.OnDied += OnDied;
        UpdateText(playerHealth.Lives, GetMaxLivesFallback());
    }

    private void OnDisable()
    {
        if (!playerHealth) return;
        playerHealth.OnLivesChanged -= UpdateText;
        playerHealth.OnDied -= OnDied;
    }

    private void UpdateText(int current, int max)
    {
        text.text = $"{current}";
    }

    private void OnDied()
    {
        text.text = "";
    }

    private int GetMaxLivesFallback() => 999; // not needed if you always call Update via event
}
