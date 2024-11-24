public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerContainer player;

    public PlayerState(PlayerStateMachine stateMachine, PlayerContainer playerContainer)
    {
        //   this.player = player;
        player = playerContainer;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter(); 
    public abstract void LogicUpdate(); 
    public abstract void Exit(); 
}