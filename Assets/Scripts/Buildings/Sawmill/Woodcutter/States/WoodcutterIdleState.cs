public class WoodcutterIdleState : WoodcutterState
{

    public override void Enter()
    {
        if (woodCutterFacade.WorkPlaceStorageFull)
        {
            ChangeState<WoodcutterWaitingState>();
            return;
        }

        ChangeState<WoodcutterSearchTreeState>();
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
    }
}
