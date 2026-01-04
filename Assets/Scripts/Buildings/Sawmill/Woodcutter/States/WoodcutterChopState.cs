using UnityEngine;

public class WoodcutterChopState : WoodcutterState
{
    private float _timer;

    public WoodcutterChopState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        _timer = 0f;

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

        if (!Ctx.HasTree)
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
            return;
        }

        var distance = Vector3.Distance(Ctx.Transform.position, Ctx.CurrentTree.transform.position);
        if (distance > Ctx.WorkSettings.ChopDistance + 0.2f)
        {
            StateMachine.ChangeState(WoodcutterStateKey.MoveToTree);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer < Ctx.ChopInterval)
            return;

        _timer = 0f;
        Ctx.CurrentTree.GetDamage(Ctx.WorkSettings.TreeDamage);
        if (!Ctx.CurrentTree.isAlive)
        {
            Ctx.ClearTree();
            StateMachine.ChangeState(WoodcutterStateKey.CollectDrops);
        }
    }

    public override void Exit()
    {
    }
}
