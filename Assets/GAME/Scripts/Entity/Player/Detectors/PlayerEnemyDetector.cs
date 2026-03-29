using System;

public class PlayerEnemyDetector : PlayerDetectorBase<BaseEnemy>
{
    private static readonly Func<BaseEnemy, bool> AliveEnemyFilter = resource => resource.isAlive;
    private readonly PlayerStateMachine playerStateMachine;

    public PlayerEnemyDetector(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine,
        IDetectionHandler<BaseEnemy> detectionHandler)
        : base(playerContainer, overlapSphereHandler, detectionHandler)
    {
        this.playerStateMachine = playerStateMachine;
    }

    public void Find() => Detect();

    protected override bool CanDetect() => playerStateMachine.CurrentStateKey != PlayerStateKey.Attack;

    protected override float GetRadius() => PlayerContainer.PlayerStats.AggroRadius;

    protected override Func<BaseEnemy, bool> GetFilter() => AliveEnemyFilter;

    protected override bool ShouldForceUpdate() => true;
}

