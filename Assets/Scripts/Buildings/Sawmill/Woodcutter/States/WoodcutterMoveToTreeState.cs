public class WoodcutterMoveToTreeState : WoodcutterState
{
    private NPCAnimator _animator;

    public WoodcutterMoveToTreeState(WoodcutterView context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        SetDestination();
    }

    public override void Tick()
    {
        if (Ctx.Agent.remainingDistance <= workSettings.ChopDistance)
        {
            StateMachine.ChangeState<WoodcutterChopState>();
        }
    }

    private void SetDestination()
    {
        Ctx.Agent.isStopped = false;
        _animator.SetMove();
        Ctx.Agent.SetDestination(woodCutterFacade.CurrentTree.transform.position);
    }

    public override void Exit()
    {
    }
}
