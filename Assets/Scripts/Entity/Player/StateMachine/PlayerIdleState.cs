public class PlayerIdleState : PlayerState
{
    
    public PlayerIdleState( PlayerStateMachine stateMachine, PlayerContainer playerContainer) : base(stateMachine, playerContainer)
    {
    }

    public override void Enter()
    {
        if (GameManager.GameState == GameState.Expedition)
        {
            stateMachine.ChangeState(PlayerStateKey.Attack);
        }
        else if(GameManager.GameState == GameState.HomeVillage)
        {
            stateMachine.ChangeState(PlayerStateKey.Farming);
        }
    }

    public override void LogicUpdate()
    {
        
    }

    public override void Exit()
    {
    }
}