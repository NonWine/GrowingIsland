using System.Collections.Generic;
using System;
using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private ResourcePartObj _targetDrop;
    private float timeAwait;
    
    public WoodcutterCollectState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        _targetDrop = null;
        if (Ctx.Agent != null)
            Ctx.Agent.isStopped = false;
    }

    public override void Tick()
    {
        

        if (_targetDrop == null)
        {
            _targetDrop = Ctx.ResourceDetector.AcquireNearestDrop();
            if(_targetDrop == null)
                return;
        }
        if (Ctx.HasWood)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
        }

        var targetPos = _targetDrop.transform.position;
        var distance = Vector3.Distance(Ctx.Transform.position, targetPos);

        if (distance > Ctx.WorkSettings.LootCollectionRadius)
        {
            if (Ctx.Agent != null)
            {
                Ctx.Agent.isStopped = false;
                Ctx.Agent.SetDestination(targetPos);
            }
            return;
        }

        // close enough to pick
        if (Ctx.Agent != null)
            Ctx.Agent.isStopped = true;

        if (_targetDrop != null && !_targetDrop.IsPicked)
        {
            _targetDrop.PickUpSilent();
            Ctx.AddWood(1);
        }

        _targetDrop = null;

        if (Ctx.CarriedWood >= Ctx.CarryCapacity)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            return;
        }

        // look for next drop same tick
        _targetDrop = Ctx.ResourceDetector.AcquireNearestDrop();
        if (_targetDrop == null)
        {
            if (Ctx.HasWood)
                StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            else
                StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
        }
    }

    public override void Exit()
    {
        _targetDrop = null;
        if (Ctx.Agent != null)
            Ctx.Agent.isStopped = false;
    }
}
