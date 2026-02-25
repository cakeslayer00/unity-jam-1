using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashEffect : MonoBehaviour
{
    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void TriggerFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float duration = 0.2f; // Длительность вспышки
        float elapsed = 0f;
        Color c = img.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Плавно уменьшаем прозрачность от 0.5 до 0
            c.a = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            img.color = c;
            yield return null;
        }
    }
}