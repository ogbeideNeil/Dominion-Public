using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStayInRangeOfPlayer : ActionNode
{
    [SerializeField]
    private float distanceToPlayer = 6f;

    private const float RaycastDistance = 100f;
    private const float YOffset = 50f;

    protected override void OnStart()
    {
        SetDestination();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance <= context.agent.stoppingDistance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }

    protected void SetDestination()
    {
        Enemy enemy = context.enemy;

        if (Vector3.Distance(enemy.transform.position, enemy.LastKnownPlayerPosition) <= distanceToPlayer)
        {
            context.agent.SetDestination(enemy.transform.position);
            return;
        }
        
        for (var i = 0; i < enemy.MaxAttempts; i++)
        {
            Vector2 point = enemy.LastKnownPlayerPosition.XZToVector2()
                .ClosestPointToCentre(enemy.transform.position.XZToVector2(), distanceToPlayer);

            var ray = new Ray(point.ToVector3XZ(enemy.LastKnownPlayerPosition.y + YOffset), Vector3.down);

            // Debug.DrawRay(ray.origin, Vector3.down * 100, Color.red, 10, true);

            if (enemy.GroundCollider.Raycast(ray, out RaycastHit raycastHit, RaycastDistance))
            {
                if (context.agent.SetDestination(point.ToVector3XZ(raycastHit.point.y)))
                {
                    return;
                }
            }
        }
    }
}