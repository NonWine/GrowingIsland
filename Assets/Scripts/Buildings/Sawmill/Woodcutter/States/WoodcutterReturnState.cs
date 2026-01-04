using UnityEngine;

public class WoodcutterReturnState : WoodcutterState
{
    private ResourcePartObj _targetDrop;

    public WoodcutterReturnState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        _targetDrop = null;
        if (Ctx.Agent != null)
        {
            Ctx.Agent.isStopped = false;
            if (Ctx.Sawmill != null)
                Ctx.Agent.SetDestination(Ctx.Sawmill.DepositPoint.position);
        }
    }

    public override void Tick()
    {
        
        if (Ctx.Agent.pathPending)
            return;

        if (Ctx.Agent.remainingDistance > Ctx.WorkSettings.DepositDistance)
            return;
        
        if (Ctx.HasWood)
        {
            var stored = Ctx.Sawmill.DepositWood(Ctx.CarriedWood);
            Ctx.RemoveWood(stored);
        }
        // Check for drops along the way if we have capacity

        if (Ctx.ResourceDetector.AcquireNearestDrop() != null)
        {
            StateMachine.ChangeState(WoodcutterStateKey.CollectDrops);
        }
        
        
        if (Ctx.StorageFull)
        {
            StateMachine.ChangeState(WoodcutterStateKey.WaitingStorage);
        }
        else
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
        }
    }

    public override void Exit()
    {
        _targetDrop = null;
    }
}
