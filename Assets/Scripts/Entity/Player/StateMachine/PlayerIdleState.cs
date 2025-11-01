public class PlayerIdleState : PlayerState
{
    
    public PlayerIdleState( PlayerStateMachine stateMachine, PlayerContainer playerContainer) : base(stateMachine, playerContainer)
    {
    }

    public override void Enter()
    {
        stateMachine.ChangeState(PlayerStateKey.Attack);
    }

    public override void LogicUpdate()
    {
        
    }

    public override void Exit()
    {
    }
}