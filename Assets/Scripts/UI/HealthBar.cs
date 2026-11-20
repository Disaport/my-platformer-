using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private IntEventSO healthChanged;
    [SerializeField] private IntEventSO maxHealthChanged;
    [SerializeField] private Image fill; // Image Type = Filled

    private int current;
    private int max = 1; // не нуль: ділимо на нього

    private void OnEnable()
    {
        maxHealthChanged.Register(SetMax);
        healthChanged.Register(Redraw);
    }

    // Рівно стільки ж рядків, скільки в OnEnable. Пропустити цей метод —
    // найчастіша помилка: ассет переживе сцену й триматиме
    // посилання на знищену смугу
    private void OnDisable()
    {
        maxHealthChanged.Unregister(SetMax);
        healthChanged.Unregister(Redraw);
    }

    private void SetMax(int value)
    {
        max = Mathf.Max(1, value);
        Apply();
    }

    private void Redraw(int value)
    {
        current = value;
        Apply();
    }

    private void Apply() => fill.fillAmount = (float)current / max;
}
