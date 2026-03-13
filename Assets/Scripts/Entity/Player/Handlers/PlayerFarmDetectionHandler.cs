using System.Collections.Generic;

public class PlayerFarmDetectionHandler : IDetectionHandler<EnvironmentResource>
{
    private readonly PlayerStateMachine playerStateMachine;
    private readonly PlayerAnimator playerAnimator;

    public PlayerFarmDetectionHandler(PlayerStateMachine playerStateMachine, PlayerAnimator playerAnimator)
    {
        this.playerStateMachine = playerStateMachine;
        this.playerAnimator = playerAnimator;
    }

    public void Handle(List<EnvironmentResource> farmObjects)
    {
        playerStateMachine.ChangeState(PlayerStateKey.Farming);
    }

    public void NoDetection()
    {
        if (playerStateMachine.CurrentStateKey == PlayerStateKey.Farming)
        {
            playerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }
}

