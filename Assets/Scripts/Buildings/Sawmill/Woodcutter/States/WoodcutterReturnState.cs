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
        Ctx.Agent.isStopped = false;
        Ctx.NpcAnimator.SetMove();
        Ctx.Agent.SetDestination(Ctx.Sawmill.DepositPoint.position);
    }

    public override void Tick()
    {

        if (Ctx.Agent.remainingDistance > Ctx.WorkSettings.DepositDistance)
            return;
        
        Ctx.TryDepositWood();
        ChangeState();
    }

    private void ChangeState()
    {
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
        Ctx.NpcAnimator.SetIdle();
    }
}
