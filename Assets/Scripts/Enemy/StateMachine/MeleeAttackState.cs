using UnityEngine;

public class MeleeAttackState : IState
{
    private readonly EnemyController ctx;
    private float timer;
    private bool struck;
    public MeleeAttackState(EnemyController ctx) => this.ctx = ctx;

    public void Enter()
    {
        ctx.Stop();
        ctx.FaceTarget();
        struck = false;
        timer = ctx.Stats.telegraphTime;
        // Телеграфування — у ВХОДІ в стан. Не в Update, не в анімації
        ctx.Animator.SetTrigger("Telegraph");
        ctx.PlayTelegraph();
    }

    public void Tick()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        if (!struck)
        {
            ctx.DealDamage();
            struck = true;
            timer = ctx.Stats.recoveryTime; // пауза відновлення
            return;
        }

        // ОБОВ'ЯЗКОВО: вихід. Стан без виходу — застиглий ворог
        ctx.Machine.ChangeState(ctx.ChaseState);
    }

    public void Exit() => ctx.Animator.ResetTrigger("Telegraph");
}
