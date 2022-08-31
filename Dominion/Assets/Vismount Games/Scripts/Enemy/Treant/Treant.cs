using UnityEngine;

public class Treant : Enemy
{
    public override EnemyType EnemyType => EnemyType.Treant;

    private new void Awake()
    {
        base.Awake();
    }

    private new void Update()
    {
        base.Update();
    }

    protected override void CheckPlayer()
    {
        if (!PlayerPositionKnown) return;

        base.CheckPlayer();
    }
}