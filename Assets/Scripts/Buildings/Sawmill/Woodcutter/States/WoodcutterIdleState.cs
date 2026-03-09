public class WoodcutterIdleState : WoodcutterState
{
    public WoodcutterIdleState(WoodcutterView context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        if (woodCutterFacade.StorageFull)
        {
            StateMachine.ChangeState<WoodcutterWaitingState>();
            return;
        }

        StateMachine.ChangeState<WoodcutterSearchTreeState>();
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
