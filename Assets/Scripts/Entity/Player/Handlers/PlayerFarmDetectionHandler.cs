using System.Collections.Generic;

public class PlayerFarmDetectionHandler : IDetectionHandler<EnvironmentResource>
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerAnimator _playerAnimator;

    public PlayerFarmDetectionHandler(PlayerStateMachine playerStateMachine, PlayerAnimator playerAnimator)
    {
        _playerStateMachine = playerStateMachine;
        _playerAnimator = playerAnimator;
    }

    public void Handle(List<EnvironmentResource> farmObjects)
    {
        if (farmObjects.Count == 0)
        {
            return;
        }

        _playerAnimator.SetFarmingAnim(farmObjects[0].ResourceType);
        _playerStateMachine.ChangeState(PlayerStateKey.Farming);
    }
}
