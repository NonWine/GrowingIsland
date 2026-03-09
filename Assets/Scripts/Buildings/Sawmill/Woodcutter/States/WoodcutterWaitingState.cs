public class WoodcutterWaitingState : WoodcutterState
{
    private readonly NPCAnimator _npcAnimator;

    public WoodcutterWaitingState(WoodcutterView context, WoodcutterStateMachine stateMachine, NPCAnimator npcAnimator) : base(context, stateMachine)
    {
        _npcAnimator = npcAnimator;
    }

    public override void Enter()
    {
        Ctx.Agent.isStopped = true;
        _npcAnimator.SetIdle();
        woodCutterFacade.Sawmill.StorageChanged += OnStorageChanged;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        if (woodCutterFacade.Sawmill != null)
            woodCutterFacade.Sawmill.StorageChanged -= OnStorageChanged;
    }

    private void OnStorageChanged(int current, int capacity)
    {
        if (woodCutterFacade.Sawmill == null)
            return;

        if (woodCutterFacade.HasWood)
        {
            StateMachine.ChangeState<WoodcutterReturnState>();
            return;
        }

        if (!woodCutterFacade.StorageFull)
            StateMachine.ChangeState<WoodcutterSearchTreeState>();
    }
}
