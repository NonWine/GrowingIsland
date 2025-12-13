using System.Collections.Generic;

public class PlayerEnemyDetectionHandler : IDetectionHandler<BaseEnemy>
{
    private readonly PlayerStateMachine _playerStateMachine;

    public PlayerEnemyDetectionHandler(PlayerStateMachine playerStateMachine)
    {
        _playerStateMachine = playerStateMachine;
    }

    public void Handle(List<BaseEnemy> enemies)
    {
        if (enemies.Count == 0)
        {
            return;
        }

        _playerStateMachine.ChangeState(PlayerStateKey.Attack);
    }

    public void NoDetection()
    {
        
    }
}
