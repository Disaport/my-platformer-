using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField, Range(0f, 2f)] private float invulnerableTime = 0.6f;
    [SerializeField, Range(0f, 20f)] private float knockbackForce = 8f;

    protected int current;
    private float invulnerableUntil;
    private Rigidbody2D body;

    public int Current => current;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ResetHealth();
    }

    public void ResetHealth() => current = maxHealth;

    // Ворогу максимум приходить з ассета — викликає EnemyController
    public void Configure(int newMax)
    {
        maxHealth = newMax;
        ResetHealth();
    }

    public void TakeDamage(int amount, Vector2 sourcePosition)
    {
        // Кадри невразливості: без них дотик знімає здоров'я ЩОКАДРУ
        if (Time.time < invulnerableUntil) return;
        invulnerableUntil = Time.time + invulnerableTime;

        current = Mathf.Max(0, current - amount);

        // Відкидання — і зворотний зв'язок, і механіка: дає час відійти
        Vector2 dir = ((Vector2)transform.position - sourcePosition).normalized;
        body.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        if (current == 0) Die();
        else OnDamaged();
    }

    // Два гачки для спадкоємців — єдине, що відрізняє ворога від гравця
    protected virtual void OnDamaged() { }
    protected virtual void Die() => gameObject.SetActive(false);
}
