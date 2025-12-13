using System;
using UnityEngine;

public class PlayerFarmDetector : PlayerDetectorBase<EnvironmentResource>
{
    private static readonly Func<EnvironmentResource, bool> AvailableFarmObjectFilter = resource => resource.isAlive;

    private readonly PlayerStateMachine _playerStateMachine;

    public PlayerFarmDetector(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        PlayerStateMachine playerStateMachine,
        IDetectionHandler<EnvironmentResource> detectionHandler)
        : base(playerContainer, overlapSphereHandler, detectionHandler)
    {
        _playerStateMachine = playerStateMachine;
    }

    public void FindFarmingResources() => Detect();

    protected override bool CanDetect() => true;

    protected override float GetRadius() => PlayerContainer.PlayerStats.RadiusFarming;

    protected override Func<EnvironmentResource, bool> GetFilter() => AvailableFarmObjectFilter;

    protected override bool ShouldForceUpdate() => true;

    protected override Vector3 DetectionPoint()
    {
        return PlayerContainer.FarmDetectionPoint.position;
    }
}
