using UnityEngine;

public class HurtState : IState
{
    private readonly EnemyController ctx;
    private float timer;
    public HurtState(EnemyController ctx) => this.ctx = ctx;

    public void Enter()
    {
        timer = ctx.Stats.hurtTime;
        ctx.Animator.SetTrigger("Hurt");
        // Швидкість НЕ чіпаємо: відкидання вже задало її в Health
    }

    public void Tick()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) ctx.Machine.ChangeState(ctx.ChaseState);
    }

    // Хто звів тригер, той його й скидає
    public void Exit() => ctx.Animator.ResetTrigger("Hurt");
}
