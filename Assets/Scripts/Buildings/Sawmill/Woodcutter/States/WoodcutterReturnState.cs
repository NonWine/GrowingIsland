public class WoodcutterReturnState : WoodcutterState
{
    private NPCAnimator _npcAnimator;

    public WoodcutterReturnState(WoodcutterView context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

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
            StateMachine.ChangeState<WoodcutterWaitingState>();
        else
            StateMachine.ChangeState<WoodcutterSearchTreeState>();
    }

    public override void Exit()
    {
        _npcAnimator.SetIdle();
    }
}
