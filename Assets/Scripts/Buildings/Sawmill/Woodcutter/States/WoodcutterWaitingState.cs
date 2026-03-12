public class WoodcutterWaitingState : WoodcutterState
{
    private readonly NPCAnimator _npcAnimator;

    public WoodcutterWaitingState(NPCAnimator npcAnimator)
    {
        _npcAnimator = npcAnimator;
    }

    public override void Enter()
    {
        view.Agent.isStopped = true;
        _npcAnimator.SetIdle();
        woodCutterFacade.StorageChanged += OnStorageChanged;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        woodCutterFacade.StorageChanged -= OnStorageChanged;
    }

    private void OnStorageChanged(int current, int capacity)
    {
        if (woodCutterFacade.HasWood)
        {
            ChangeState<WoodcutterReturnState>();
            return;
        }

        if (!woodCutterFacade.WorkPlaceStorageFull)
            ChangeState<WoodcutterSearchTreeState>();
    }
}
