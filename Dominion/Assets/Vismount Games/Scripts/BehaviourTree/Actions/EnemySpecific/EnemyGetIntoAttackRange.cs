using TheKiwiCoder;
using UnityEngine;

public class EnemyGetIntoAttackRange : ActionNode
{
    [SerializeField]
    private float timeout = 5f;

    private float timePassed;

    protected override void OnStart()
    {
        timePassed = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        timePassed += Time.deltaTime;

        if (timePassed > timeout)
        {
            return State.Failure;
        }

        Enemy enemy = context.enemy;
        float distance = Vector3.Distance(enemy.transform.position, enemy.LastKnownPlayerPosition);

        if (distance <= enemy.CurrentAttackDistance)
        {
            context.agent.destination = enemy.transform.position;
            return State.Success;
        }

        context.agent.destination = enemy.LastKnownPlayerPosition;

        return State.Running;
    }
}