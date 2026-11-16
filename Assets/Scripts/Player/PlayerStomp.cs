using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [SerializeField] private Transform stompPoint;
    [SerializeField, Range(0.1f, 1f)] private float radius = 0.3f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 10;
    [SerializeField] private float bounceForce = 9f;
    // Поріг падіння: навіть на рівній землі фізика дає швидкість на кшталт −0.0001,
    // і з умовою «< 0» удар «зверху» спрацьовував від звичайного дотику збоку
    [SerializeField, Range(0.1f, 5f)] private float minFallSpeed = 1f;

    private Rigidbody2D body;

    private void Awake() => body = GetComponent<Rigidbody2D>();

    private void Update()
    {
        // Б'ємо тільки якщо справді падаємо: інакше ворог гине від дотику збоку
        if (body.linearVelocity.y > -minFallSpeed) return;

        Collider2D hit = Physics2D.OverlapCircle(stompPoint.position, radius, enemyLayer);
        if (hit == null) return;

        // Ноги героя мають бути ВИЩЕ за середину ворога — тобто удар саме згори
        if (stompPoint.position.y < hit.bounds.center.y) return;

        if (hit.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, transform.position);
            body.linearVelocity = new Vector2(body.linearVelocity.x, bounceForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (stompPoint == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(stompPoint.position, radius);
    }
}
