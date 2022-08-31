using TheKiwiCoder;

public class LockAgentRotation : ActionNode
{
    protected override void OnStart()
    {
        context.agent.updateRotation = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}