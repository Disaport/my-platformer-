using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Дані")]
    [SerializeField] private EnemyStatsSO stats;

    [Header("Точки")]
    [SerializeField] private Transform groundCheck;  // під ступнею
    [SerializeField] private Transform edgeCheck;    // перед носком, НИЖЧЕ ступні
    [SerializeField] private Transform wallCheck;    // перед носком, на рівні тулуба
    [SerializeField] private Transform attackPoint;  // звідки йде удар

    [Header("Шари")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask damageableLayers;

    [Header("Звук")]
    [SerializeField] private AudioSource audioSource;

    [Header("Стрільба")]
    [SerializeField] private Projectile projectilePrefab;

    public EnemyStatsSO Stats => stats;
    public Rigidbody2D Body { get; private set; }
    public Animator Animator { get; private set; }
    public StateMachine Machine { get; private set; }
    public Transform Target { get; private set; }

    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public HurtState HurtState { get; private set; }
    public DeadState DeadState { get; private set; }
    public IState AttackState { get; private set; }

    public int Facing { get; private set; } = 1;

    private ObjectPool<Projectile> pool;

    [Header("Налагодження (лише для читання)")]
    [SerializeField] private string debugState;
    [SerializeField] private float debugDistance;
    [SerializeField] private bool debugSameLevel;
    [SerializeField] private bool debugAtEdge;
    [SerializeField] private bool debugAtWall;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        Machine = new StateMachine();

        // Префаб не може зберігати посилання на об'єкт сцени —
        // гравця шукаємо в коді. Та сама пастка, що з камерою на Л3
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        // Максимум здоров'я приходить з ассета
        GetComponent<Health>().Configure(stats.maxHealth);

        // Вигляд теж приходить з ассета — тому другий тип ворога
        // не потребує ні нових спрайтів, ні нових анімацій
        GetComponentInChildren<SpriteRenderer>().color = stats.tint;
        ApplyScale();

        PatrolState = new PatrolState(this);
        ChaseState = new ChaseState(this);
        HurtState = new HurtState(this);
        DeadState = new DeadState(this);
        // Вибір атаки — теж за ассетом, контролер для стрільця не змінюється
        AttackState = stats.ranged
            ? (IState)new RangedAttackState(this)
            : new MeleeAttackState(this);

        pool = new ObjectPool<Projectile>(
            createFunc: () => Instantiate(projectilePrefab),
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => Destroy(p.gameObject),
            defaultCapacity: 20);
    }

    private void Start() => Machine.ChangeState(PatrolState);

    private void Update()
    {
        Machine.Tick();
        // Видно в Inspector під час гри: який стан і чому
        debugState = Machine.Current?.GetType().Name;
        debugDistance = DistanceToTarget;
        debugSameLevel = TargetOnSameLevel;
        debugAtEdge = AtEdge();
        debugAtWall = AtWall();
    }

    // Відстань міряємо ПО ГОРИЗОНТАЛІ: висоту окремо перевіряє
    // TargetOnSameLevel, тому пряма відстань тут тільки заважала б
    public float DistanceToTarget =>
        Mathf.Abs(Target.position.x - transform.position.x);

    // Рівень порівнюємо по СТУПНЯХ, а не по початках координат
    public bool TargetOnSameLevel =>
        Mathf.Abs(Target.position.y - groundCheck.position.y)
        <= stats.verticalTolerance;

    public void Move(float speed)
    {
        Body.linearVelocity = new Vector2(Facing * speed, Body.linearVelocity.y);
        Animator.SetFloat("Speed", Mathf.Abs(Body.linearVelocity.x));
    }

    public void Stop()
    {
        Body.linearVelocity = new Vector2(0f, Body.linearVelocity.y);
        Animator.SetFloat("Speed", 0f);
    }

    public void Flip()
    {
        Facing = -Facing;
        ApplyScale();
    }

    // Масштаб і дзеркалення задаються разом: інакше розворот
    // щоразу скидав би розмір, заданий ассетом
    private void ApplyScale() =>
        transform.localScale = new Vector3(stats.sizeScale * Facing, stats.sizeScale, 1f);

    // Названа константа, а не магічне число: її не крутить дизайнер
    private const float FacingDeadZone = 0.1f;

    public void FaceTarget()
    {
        float dx = Target.position.x - transform.position.x;
        // Без мертвої зони ворог, що стоїть рівно під гравцем,
        // розвертається щокадру від дрібних коливань координати
        if (Mathf.Abs(dx) < FacingDeadZone) return;
        int dir = dx > 0f ? 1 : -1;
        if (dir != Facing) Flip();
    }

    // Радіус датчиків: структурна величина, дизайнер її не крутить
    private const float ProbeRadius = 0.1f;

    // Ворог стоїть на землі, а не летить
    public bool IsGrounded =>
        Physics2D.OverlapCircle(groundCheck.position, ProbeRadius, groundLayer);

    // Під носком НЕМАЄ землі — попереду край платформи
    public bool AtEdge() =>
        !Physics2D.OverlapCircle(edgeCheck.position, ProbeRadius, groundLayer);

    // Перед носком Є перешкода — попереду стіна
    public bool AtWall() =>
        Physics2D.OverlapCircle(wallCheck.position, ProbeRadius, groundLayer);

    public void PlayTelegraph()
    {
        if (stats.telegraphSfx != null)
            audioSource.PlayOneShot(stats.telegraphSfx);
    }

    // Ворог не знає, кого б'є: питає про здатність отримувати урон
    public void DealDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position, stats.attackRadius, damageableLayers);
        if (hit != null && hit.TryGetComponent(out IDamageable target))
            target.TakeDamage(stats.damage, transform.position);
    }

    public void Shoot()
    {
        Projectile p = pool.Get();
        // Стан скидаємо ЯВНО: перевикористаний снаряд пам'ятає старий
        p.transform.position = attackPoint.position;
        p.OnFinished = Release;
        p.Launch(new Vector2(Facing, 0f), stats.damage);
    }

    private void Release(Projectile p)
    {
        p.OnFinished = null;
        pool.Release(p);
    }

    // Щоб бачити радіуси й датчики у вікні Scene
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, stats != null ? stats.attackRadius : 0.5f);
        }
        Gizmos.color = Color.yellow;
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.position, ProbeRadius);
        if (edgeCheck != null) Gizmos.DrawWireSphere(edgeCheck.position, ProbeRadius);
        if (wallCheck != null) Gizmos.DrawWireSphere(wallCheck.position, ProbeRadius);
    }
}
