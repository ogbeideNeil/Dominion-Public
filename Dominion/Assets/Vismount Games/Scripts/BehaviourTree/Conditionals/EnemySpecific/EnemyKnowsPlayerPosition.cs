using TheKiwiCoder;

public class EnemyKnowsPlayerPosition : ConditionalNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        return context.enemy.PlayerPositionKnown
            ? State.Success
            : State.Failure;
    }
}
