public class WoodcutterMoveToTreeState : WoodcutterState
{
    public WoodcutterMoveToTreeState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        SetDestination();
    }

    public override void Tick()
    {
        if (Ctx.Agent.remainingDistance <= Ctx.WorkSettings.ChopDistance)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ChopTree);
        }
    }

    private void SetDestination()
    {
        Ctx.Agent.isStopped = false;
        Ctx.NpcAnimator.SetMove();
        Ctx.Agent.SetDestination(Ctx.CurrentTree.transform.position);
    }

    public override void Exit()
    {
        
    }
}
