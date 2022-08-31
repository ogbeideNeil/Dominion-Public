using TheKiwiCoder;
using UnityEngine;

public class EnemyAttackPlayer : ActionNode
{
    protected override void OnStart()
    {
        _ = context.enemy.Attack();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return context.enemy.AttackFinished
            ? State.Success
            : State.Running;
    }
}