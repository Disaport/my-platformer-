using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{
    private bool used; // два колайдери гравця — один перехід

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.TryGetComponent(out PlayerController _)) return;
        used = true;
        // Увесь розрахунок «що далі» живе у Flow, а не тут
        Flow.NextLevel();
    }
}
