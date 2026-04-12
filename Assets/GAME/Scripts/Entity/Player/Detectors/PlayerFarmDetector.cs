using System;
using UnityEngine;

public class PlayerFarmDetector : PlayerDetectorBase<EnvironmentPropObjectView>
{
    private static readonly Func<EnvironmentPropObjectView, bool> AvailableFarmObjectFilter = resource => resource.IsAlive;

    private readonly PlayerStateMachine playerStateMachine;

    public PlayerFarmDetector(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine,
        IDetectionHandler<EnvironmentPropObjectView> detectionHandler)
        : base(playerContainer, overlapSphereHandler, detectionHandler)
    {
        this.playerStateMachine = playerStateMachine;
    }

    public void FindFarmingResources() => Detect();

    protected override bool CanDetect() => playerStateMachine.CurrentStateKey != PlayerStateKey.Attack;

    protected override float GetRadius() => PlayerContainer.PlayerStats.RadiusFarming;

    protected override Func<EnvironmentPropObjectView, bool> GetFilter() => AvailableFarmObjectFilter;

    protected override bool ShouldForceUpdate() => true;

    protected override Vector3 DetectionPoint()
    {
        return PlayerContainer.transform.position;
    }
}

