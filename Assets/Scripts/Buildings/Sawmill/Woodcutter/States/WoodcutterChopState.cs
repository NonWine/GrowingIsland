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
        Ctx.Agent.isStopped = true;
        Ctx.NpcAnimator.SetAttack();
    }

    public override void Tick()
    {
        
        _timer += Time.deltaTime;
        if (_timer < Ctx.ChopInterval)
            return;

        _timer = 0f;
        Ctx.CurrentTree.GetDamage(Ctx.WorkSettings.TreeDamage);
        
        if (!Ctx.CurrentTree.isAlive)
        {
            Ctx.ClearTree();
            StateMachine.ChangeState(WoodcutterStateKey.CollectState);
        }
    }
    
    public override void Exit()
    {
        Ctx.NpcAnimator.SetIdle();
    }
}
