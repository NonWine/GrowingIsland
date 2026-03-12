using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private readonly IWoodcutterSensor _sensor;
    private readonly WoodcutterWorkSettings woodcutterWorkSettings;
    private float _timer;
    private const float TimeAwait = 0.5f;

    public WoodcutterCollectState(IWoodcutterSensor sensor, WoodcutterWorkSettings workSettings)
    {
        _sensor = sensor;
        woodcutterWorkSettings = workSettings;
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

        if (woodcutterWorkSettings.CarryCapacity <= woodCutterFacade.CarriedWood)
            ChangeState<WoodcutterReturnState>();
        else
            ChangeState<WoodcutterSearchTreeState>();
    }

    private void PickUpWood()
    {
        foreach (var drop in _sensor.GetDropsInRadius(15))
        {
            drop.PickUp(view.transform, CollectStrategyType.NPC, 0);
            woodCutterFacade.AddWood(1);
        }
    }

    public override void Exit()
    {
        _timer = 0f;
    }
}
