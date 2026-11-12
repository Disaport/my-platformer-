using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private Coroutine current;

    public void Stop(float duration = 0.05f)
    {
        // Якщо пауза вже триває — ІГНОРУЄМО новий виклик, а не перезапускаємо.
        // Перервана корутина не дійде до timeScale = 1, і гра замерзне назавжди
        if (current != null) return;
        current = StartCoroutine(Routine(duration));
    }

    private IEnumerator Routine(float duration)
    {
        Time.timeScale = 0f;
        // WaitForSecondsRealtime, бо WaitForSeconds при timeScale = 0 НЕ спливе
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        current = null;
    }
}