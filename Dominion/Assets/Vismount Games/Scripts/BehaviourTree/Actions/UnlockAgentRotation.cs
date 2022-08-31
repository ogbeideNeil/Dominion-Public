using TheKiwiCoder;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class UnlockAgentRotation : ActionNode
{
    protected override void OnStart()
    {
        context.agent.updateRotation = true;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}