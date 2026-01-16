public class WoodcutterIdleState : WoodcutterState
{
    public WoodcutterIdleState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        if (Ctx.StorageFull)
        {
            StateMachine.ChangeState(WoodcutterStateKey.WaitingStorage);
            return;
        }

        StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
