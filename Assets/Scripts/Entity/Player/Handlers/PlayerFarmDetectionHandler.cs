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
        _playerStateMachine.ChangeState(PlayerStateKey.Farming);
    }

    public void NoDetection()
    {
        if (_playerStateMachine.CurrentStateKey == PlayerStateKey.Farming)
        {
            _playerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }
}
