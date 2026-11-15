using UnityEngine;

// Усі числа ворога — тут. Новий тип ворога = новий ассет, а не новий клас
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Рух")]
    [Range(0.5f, 8f)] public float patrolSpeed = 1.5f;
    [Range(0.5f, 10f)] public float chaseSpeed = 3f;

    [Header("Відстані")]
    [Range(1f, 15f)] public float chaseRange = 6f;
    [Range(0.5f, 8f)] public float attackRange = 1.2f;
    [Range(0.2f, 2f)] public float attackRadius = 0.6f;
    // Наскільки гравець може бути вище або нижче, щоб ворог
    // усе ще вважав його «своєю» ціллю
    [Range(0.5f, 6f)] public float verticalTolerance = 1.5f;
    // Поріг виходу з переслідування більший за поріг входу (гістерезис)
    [Range(1f, 2f)] public float chaseExitFactor = 1.2f;
    // Скільки ворог іще біжить за ціллю, яку втратив
    [Range(0.1f, 3f)] public float loseTargetTime = 1f;
    // Ближче за цю відстань ворог уже не підбігає
    [Range(0.1f, 3f)] public float stopDistance = 0.6f;

    [Header("Бій")]
    [Range(1, 200)] public int maxHealth = 30;
    [Range(1, 50)] public int damage = 10;
    [Range(0.05f, 2f)] public float telegraphTime = 0.45f;
    [Range(0.1f, 2f)] public float recoveryTime = 0.35f;
    [Range(0.1f, 2f)] public float hurtTime = 0.3f;

    [Header("Вигляд")]
    [Tooltip("Загальний відтінок спрайта. Білий — без змін")]
    public Color tint = Color.white;
    [Range(0.5f, 2f)] public float sizeScale = 1f;

    [Header("Подача")]
    public AudioClip telegraphSfx;

    [Header("Тип атаки")]
    public bool ranged = false; // true — стрілець
}
