using System.Collections.Generic;

public class PlayerEnemyDetectionHandler : IDetectionHandler<BaseEnemy>
{
    private readonly PlayerStateMachine playerStateMachine;

    public PlayerEnemyDetectionHandler(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    public void Handle(List<BaseEnemy> enemies)
    {
        if (enemies.Count == 0)
        {
            return;
        }

        playerStateMachine.ChangeState(PlayerStateKey.Attack);
    }

    public void NoDetection()
    {
        
    }
}

