using System;
using TheKiwiCoder;
using UnityEngine;

public class EnemyDistanceToLastKnownPlayerPosition : ConditionalNode
{
    private enum Comparator
    {
        Equal,
        LessThan,
        GreaterThan
    }

#pragma warning disable CS0649
    [SerializeField]
    private Comparator comparator;

    [SerializeField]
    private float value;
#pragma warning restore CS0649

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float distance = Vector3.Distance(
            context.enemy.transform.position,
            context.enemy.LastKnownPlayerPosition);

        return comparator switch
        {
            Comparator.Equal => Equal(distance, value),
            Comparator.LessThan => LessThan(distance, value),
            Comparator.GreaterThan => GreaterThan(distance, value),
            _ => throw new ArgumentOutOfRangeException($"Comparator {comparator} is not implemented")
        };
    }

    private State Equal(float a, float b)
    {
        return a.Equals(b)
            ? State.Success
            : State.Failure;
    }

    private State LessThan(float a, float b)
    {
        return a < b
            ? State.Success
            : State.Failure;
    }

    private State GreaterThan(float a, float b)
    {
        return a > b
            ? State.Success
            : State.Failure;
    }
}
