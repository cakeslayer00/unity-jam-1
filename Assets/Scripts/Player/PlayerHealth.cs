using UnityEngine;

using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;

    public int Lives { get; private set; }

    public event Action<int, int> OnLivesChanged; // (current, max)
    public event Action OnDied;

    private bool dead;

    private void Awake()
    {
        Lives = maxLives;
        OnLivesChanged?.Invoke(Lives, maxLives);
    }

    public void TakeHit(int damage = 1)
    {
        if (dead) return;

        Lives = Mathf.Max(0, Lives - damage);
        OnLivesChanged?.Invoke(Lives, maxLives);

        if (Lives == 0)
        {
            dead = true;
            OnDied?.Invoke();
        }
    }

    public void ResetLives()
    {
        dead = false;
        Lives = maxLives;
        OnLivesChanged?.Invoke(Lives, maxLives);
    }
}
