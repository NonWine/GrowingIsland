using Zenject;

public class WoodcutterReturnState : WoodcutterState
{
    [Inject] private NPCAnimator _npcAnimator;

    public override void Enter()
    {
        view.Agent.isStopped = false;
        _npcAnimator.SetMove();
        view.Agent.SetDestination(woodCutterFacade.DepositPoint.position);
    }

    public override void Tick()
    {
        if (view.Agent.remainingDistance > workSettings.DepositDistance)
            return;

        ChangeState<WoodcutterDepositState>();
    }

    public override void Exit()
    {
        _npcAnimator.SetIdle();
    }
}
