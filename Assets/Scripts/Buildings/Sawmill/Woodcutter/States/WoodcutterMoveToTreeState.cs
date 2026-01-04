public class WoodcutterMoveToTreeState : WoodcutterState
{
    public WoodcutterMoveToTreeState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        if (Ctx.Agent == null)
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
            return;
        }

        Ctx.Agent.isStopped = false;
        SetDestination();
    }

    public override void Tick()
    {
        if (Ctx.StorageFull && !Ctx.HasWood)
        {
            StateMachine.ChangeState(WoodcutterStateKey.WaitingStorage);
            return;
        }

        if (!Ctx.HasTree)
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
            return;
        }

        if (Ctx.Agent.pathPending)
            return;

        if (Ctx.Agent.remainingDistance <= Ctx.WorkSettings.ChopDistance)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ChopTree);
        }
    }

    private void SetDestination()
    {
        if (Ctx.HasTree)
        {
            Ctx.Agent.SetDestination(Ctx.CurrentTree.transform.position);
        }
    }

    public override void Exit()
    {
    }
}
