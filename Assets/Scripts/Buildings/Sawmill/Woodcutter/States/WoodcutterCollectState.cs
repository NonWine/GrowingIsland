using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private readonly IWoodcutterSensor _sensor;
    private float _timer;
    private const float TimeAwait = 0.5f;

    public WoodcutterCollectState(WoodcutterView context, WoodcutterStateMachine stateMachine, IWoodcutterSensor sensor) : base(context, stateMachine)
    {
        _sensor = sensor;
    }

    public override void Enter()
    {
    }

    public override void Tick()
    {
        _timer += Time.deltaTime;
        if (_timer < TimeAwait)
            return;

        PickUpWood();

        if (woodCutterFacade.CarryCapacity <= woodCutterFacade.CarriedWood)
            StateMachine.ChangeState<WoodcutterReturnState>();
        else
            StateMachine.ChangeState<WoodcutterSearchTreeState>();
    }

    private void PickUpWood()
    {
        foreach (var drop in _sensor.GetDropsInRadius(15))
        {
            drop.PickUp(Ctx.transform, CollectStrategyType.NPC, 0);
            woodCutterFacade.AddWood(1);
        }
    }

    public override void Exit()
    {
        _timer = 0f;
    }
}
