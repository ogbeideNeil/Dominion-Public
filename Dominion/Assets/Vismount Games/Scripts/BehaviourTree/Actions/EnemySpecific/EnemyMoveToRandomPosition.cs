using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMoveToRandomPosition : ActionNode
{
    private const float RaycastDistance = 100f;
    private const float YOffset = 50f;

    protected override void OnStart()
    {
        Enemy enemy = context.enemy;
        for (var i = 0; i < enemy.MaxAttempts; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * 0.5f * enemy.WanderRadius;

            var newPosition = new Vector3(
                enemy.StartPosition.x + randomPoint.x,
                enemy.StartPosition.y + YOffset,    // ray origin only (downwards direction)
                enemy.StartPosition.z + randomPoint.y);

            // Debug.DrawRay(newPosition, Vector3.down * 100, Color.red, 10, true);

            Vector3 distance = enemy.transform.position - newPosition;

            if (distance.XZToVector2().magnitude >= enemy.MinMoveDistance)
            {
                var ray = new Ray(newPosition, Vector3.down);

                if (enemy.GroundCollider.Raycast(ray, out RaycastHit raycastHit, RaycastDistance))
                {
                    if (context.agent.SetDestination(newPosition.NewY(raycastHit.point.y)))
                    {
                        return;
                    }
                }
            }
        }
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
}