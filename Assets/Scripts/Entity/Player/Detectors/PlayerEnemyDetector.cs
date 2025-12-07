using System;

public class PlayerEnemyDetector : PlayerDetectorBase<BaseEnemy>
{
    private static readonly Func<BaseEnemy, bool> AliveEnemyFilter = resource => resource.isAlive;
    private readonly PlayerStateMachine _playerStateMachine;

    public PlayerEnemyDetector(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine,
        IDetectionHandler<BaseEnemy> detectionHandler)
        : base(playerContainer, overlapSphereHandler, detectionHandler)
    {
        _playerStateMachine = playerStateMachine;
    }

    public void Find() => Detect();

    protected override bool CanDetect() => _playerStateMachine.CurrentStateKey != PlayerStateKey.Attack;

    protected override float GetRadius() => PlayerContainer.PlayerStats.AggroRadius;

    protected override Func<BaseEnemy, bool> GetFilter() => AliveEnemyFilter;

    protected override bool ShouldForceUpdate() => true;
}
