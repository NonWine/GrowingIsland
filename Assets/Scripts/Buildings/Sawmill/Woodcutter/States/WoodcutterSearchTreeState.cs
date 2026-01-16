using UnityEngine;

public class WoodcutterSearchTreeState : WoodcutterState
{
    private float _nextSearchTime;

    public WoodcutterSearchTreeState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        _nextSearchTime = 0f;

        if (Ctx.Agent != null)
            Ctx.Agent.isStopped = true;
    }

    public override void Tick()
    {
        if (Ctx.StorageFull && !Ctx.HasWood)
        {
            StateMachine.ChangeState(WoodcutterStateKey.WaitingStorage);
            return;
        }

        if (Time.time < _nextSearchTime)
            return;

        if (Ctx.TryAcquireTree(out var tree))
        {
            Ctx.SetTree(tree);
            StateMachine.ChangeState(WoodcutterStateKey.MoveToTree);
        }
        else
        {
            _nextSearchTime = Time.time + Mathf.Max(0.05f, Ctx.WorkSettings.RetargetCooldown);
        }
    }

    public override void Exit()
    {
    }
}
