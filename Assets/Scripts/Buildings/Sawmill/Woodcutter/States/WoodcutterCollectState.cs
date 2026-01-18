using System.Collections.Generic;
using System;
using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private ResourcePartObj _targetDrop;
    private float timer;
    private float timeAwait = 0.5f;
    
    public WoodcutterCollectState(WoodcutterContext context, WoodcutterStateMachine stateMachine) : base(context, stateMachine)
    {
    }

    public override void Enter()
    {

    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= timeAwait)
        {
            PickUpWood();
            if (Ctx.CarryCapacity <= Ctx.CarriedWood)
            {
                StateMachine.ChangeState(WoodcutterStateKey.ReturnToSawmill);
            }
            else
            {
                StateMachine.ChangeState(WoodcutterStateKey.SearchTree);
            }
        }

    }

    private void PickUpWood()
    {
        foreach (var resourcePartObj in Ctx.Sensor.GetDropsInRadius(15))
        {
            Debug.Log(resourcePartObj.name);
            resourcePartObj.PickUp(Ctx.Transform, CollectStrategyType.NPC, 0);
            Ctx.AddWood(1);
        }
    }
    public override void Exit()
    {
        timer = 0f;
    }
}
