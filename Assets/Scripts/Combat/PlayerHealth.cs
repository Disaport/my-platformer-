using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerRespawn respawn; // з лабораторної 4

    [Header("Канали подій")]
    [SerializeField] private IntEventSO healthChanged;
    [SerializeField] private IntEventSO maxHealthChanged;

    // Start, а НЕ Awake: усі OnEnable у сцені вже відпрацювали,
    // тобто HUD уже підписався і стартові числа до нього дійдуть
    private void Start()
    {
        maxHealthChanged.Raise(Max);
        healthChanged.Raise(Current);
    }

    // ResetHealth викликається ще в Awake, коли канали можуть бути
    // не призначені в тестовій сцені — тому перевірка на null
    protected override void OnHealthChanged()
    {
        if (healthChanged != null) healthChanged.Raise(Current);
    }

    protected override void Die()
    {
        respawn.Respawn();
        ResetHealth(); // ResetHealth сам викличе OnHealthChanged
    }
}
