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
            PickUpWood();
            Ctx.ClearTree();
            ChangeState();
        }
    }

    private void PickUpWood()
    {
        foreach (var resourcePartObj in Ctx.ResourceDetector.GetDropsWithFallback(15))
        {
            resourcePartObj.PickUp(Ctx.Transform, CollectStrategyType.NPC, 0);
            Ctx.AddWood(1);
        }
    }

    private void ChangeState()
    {
        if (Ctx.CarryCapacity <= Ctx.CarriedWood)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
        }
        else
        {
            StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
        }
    }

    public override void Exit()
    {
        Ctx.NpcAnimator.SetIdle();
    }
}
