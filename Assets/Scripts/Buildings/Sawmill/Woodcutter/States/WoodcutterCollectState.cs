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
        
        if (!TryFindWood()) return;
        

        //move for wood
        var distance = Vector3.Distance(Ctx.Transform.position,  _targetDrop.transform.position);
        if (distance > Ctx.WorkSettings.LootCollectionRadius)
        {
            Ctx.Agent.isStopped = false;
            Ctx.Agent.SetDestination( _targetDrop.transform.position);
            return;
        }


        TryPickUp();

        CheckCapacity();

    }

    private bool CheckCapacity()
    {
        if (Ctx.CarriedWood >= Ctx.CarryCapacity)
        {
            StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            return true;
        }

        return false;
    }

    private void TryPickUp()
    {
        Ctx.Agent.isStopped = true;
        if (_targetDrop.PickUpSilent())
        {
            Ctx.AddWood(1);
        }
        _targetDrop = null;
    }

    private bool TryFindWood()
    {
        if (_targetDrop != null) return true;
        
        _targetDrop = Ctx.ResourceDetector.AcquireNearestDrop();
        
        if(_targetDrop == null)
        {
            if (Ctx.HasWood)
                StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            else
                StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
        }

        return true;
    }

    public override void Exit()
    {
        _targetDrop = null;
        if (Ctx.Agent != null)
            Ctx.Agent.isStopped = false;
    }
}
