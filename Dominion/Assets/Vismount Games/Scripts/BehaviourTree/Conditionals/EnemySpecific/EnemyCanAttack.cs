using TheKiwiCoder;

public class EnemyCanAttack : ConditionalNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return context.enemy.CanAttack
            ? State.Success
            : State.Failure;
    }
}