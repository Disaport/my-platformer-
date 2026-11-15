public class PatrolState : IState
{
    private readonly EnemyController ctx;
    public PatrolState(EnemyController ctx) => this.ctx = ctx;

    public void Enter() { }

    public void Tick()
    {
        // Спершу розворот, потім рух. Розвертаємось лише стоячи
        // на землі: у повітрі AtEdge() істинний в обидва боки
        if (ctx.IsGrounded && (ctx.AtEdge() || ctx.AtWall()))
            ctx.Flip();

        ctx.Move(ctx.Stats.patrolSpeed);

        // Вихід: гравець близько і на тому ж рівні
        if (ctx.DistanceToTarget <= ctx.Stats.chaseRange && ctx.TargetOnSameLevel)
            ctx.Machine.ChangeState(ctx.ChaseState);
    }

    public void Exit() { }
}
