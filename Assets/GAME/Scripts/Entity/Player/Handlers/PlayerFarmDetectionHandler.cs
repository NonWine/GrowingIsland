using System.Collections.Generic;
using UnityEngine;

public class PlayerFarmDetectionHandler : IDetectionHandler<EnvironmentPropObjectView>
{
    private readonly PlayerStateMachine playerStateMachine;
    private readonly PlayerContainer playerContainer;
    private readonly PlayerFarmTargetTracker farmTargetTracker;

    public PlayerFarmDetectionHandler(
        PlayerStateMachine playerStateMachine,
        PlayerContainer playerContainer,
        PlayerFarmTargetTracker farmTargetTracker)
    {
        this.playerStateMachine = playerStateMachine;
        this.playerContainer = playerContainer;
        this.farmTargetTracker = farmTargetTracker;
    }

    public void Handle(List<EnvironmentPropObjectView> farmObjects)
    {
        if (farmObjects == null || farmObjects.Count == 0)
        {
            return;
        }

        EnvironmentPropObjectView nearestTarget = GetNearestTarget(farmObjects);
        if (nearestTarget == null)
        {
            return;
        }

        farmTargetTracker.SetTarget(nearestTarget);
        

        playerStateMachine.ChangeState(PlayerStateKey.Farming);
    }

    public void NoDetection()
    {
        if (ShouldKeepCurrentTarget())
        {
            return;
        }

        farmTargetTracker.Clear();

        if (playerStateMachine.CurrentStateKey == PlayerStateKey.Farming)
        {
            playerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }

    private EnvironmentPropObjectView GetNearestTarget(List<EnvironmentPropObjectView> farmObjects)
    {
        EnvironmentPropObjectView nearestTarget = null;
        float nearestSqrDistance = float.MaxValue;
        Vector3 origin = playerContainer.transform.position;

        foreach (EnvironmentPropObjectView farmObject in farmObjects)
        {
            if (farmObject == null || !farmObject.IsAlive)
            {
                continue;
            }

            float sqrDistance = (farmObject.transform.position - origin).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestTarget = farmObject;
            }
        }

        return nearestTarget;
    }

    private bool ShouldKeepCurrentTarget()
    {
        EnvironmentPropObjectView currentTarget = farmTargetTracker.CurrentTarget;
        if (currentTarget == null || !currentTarget.IsAlive)
        {
            return false;
        }

        Vector3 detectionPoint = playerContainer.transform.position;
        Vector3 targetPosition = currentTarget.transform.position;
        targetPosition.y = detectionPoint.y;

        float farmingRadius = playerContainer.PlayerStats.RadiusFarming;
        return (targetPosition - detectionPoint).sqrMagnitude <= farmingRadius * farmingRadius;
    }
}
