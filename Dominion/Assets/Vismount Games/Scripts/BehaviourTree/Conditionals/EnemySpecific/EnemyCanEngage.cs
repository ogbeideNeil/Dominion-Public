using TheKiwiCoder;

public class EnemyCanEngage : ConditionalNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return context.enemy.EngagePlayer
            ? State.Success
            : State.Failure;
    }
}