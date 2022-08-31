public class EnemyKeepDistanceFromPlayer : EnemyMoveTowardsLastKnownPlayerPosition
{
    protected override State OnUpdate()
    {
        SetDestination();

        return State.Success;
    }
}