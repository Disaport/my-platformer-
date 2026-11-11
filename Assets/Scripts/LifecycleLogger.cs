using UnityEngine;

public class LifecycleLogger : MonoBehaviour
{
    private const string DefaultLabelValue = "Object";

    [Header("Підпис, щоб розрізняти об'єкти в Console")]
    [SerializeField] private string label = DefaultLabelValue;

    private int updateCount;

    // Awake — налаштовуємо СЕБЕ. Інших об'єктів тут ще може не існувати
    private void Awake()
    {
        if (label == DefaultLabelValue)
        {
            label = this.name;
        }
        Log("Awake");
    }

    // OnEnable — підписуємось на події
    private void OnEnable() => Log("OnEnable");

    // Start — звертаємось до ІНШИХ. Усі Awake у сцені вже відпрацювали
    private void Start() => Log("Start");

    private void Update()
    {
        updateCount++;
        // Друкуємо лише перші три кадри, інакше Console захлинеться
        if (updateCount <= 3)
            Log($"Update #{updateCount}");
    }

    private void OnDisable() => Log("OnDisable");
    private void OnDestroy() => Log("OnDestroy");

    // Time.frameCount — номер кадру. Саме він доводить порядок викликів
    private void Log(string stage)
    {
        Debug.Log($"[кадр {Time.frameCount:D3}] {label} → {stage}", this);
    }
}