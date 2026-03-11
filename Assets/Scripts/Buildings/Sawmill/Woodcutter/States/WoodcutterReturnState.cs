using Zenject;

public class WoodcutterReturnState : WoodcutterState
{
    [Inject] private NPCAnimator _npcAnimator;

    public override void Enter()
    {
        Ctx.Agent.isStopped = false;
        _npcAnimator.SetMove();
        Ctx.Agent.SetDestination(woodCutterFacade.Sawmill.DepositPoint.position);
    }

    public override void Tick()
    {
        if (Ctx.Agent.remainingDistance > workSettings.DepositDistance)
            return;

        woodCutterFacade.TryDepositWood();
        ChangeNext();
    }

    private void ChangeNext()
    {
        if (woodCutterFacade.StorageFull)
            ChangeState<WoodcutterWaitingState>();
        else
            ChangeState<WoodcutterSearchTreeState>();
    }

    public override void Exit()
    {
        _npcAnimator.SetIdle();
    }
}
