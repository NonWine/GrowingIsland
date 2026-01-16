public class WoodcutterWaitingState : WoodcutterState
{
    public WoodcutterWaitingState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        Ctx.Agent.isStopped = true;
        Ctx.NpcAnimator.SetIdle();
        Ctx.Sawmill.StorageChanged += OnStorageChanged;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        if (Ctx.Sawmill != null)
            Ctx.Sawmill.StorageChanged -= OnStorageChanged;
    }

    private void OnStorageChanged(int current, int capacity)
    {
        if (Ctx.Sawmill == null)
            return;

        if (Ctx.HasWood)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            return;
        }

        if (!Ctx.StorageFull)
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
        }
    }
}
