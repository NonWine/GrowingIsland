using UnityEngine;

public class WoodcutterCollectState : WoodcutterState
{
    private readonly IWoodcutterSensor sensor;
    private readonly WoodcutterWorkSettings woodcutterWorkSettings;
    private float timer;
    private const float TimeAwait = 0.5f;

    public WoodcutterCollectState(IWoodcutterSensor sensor, WoodcutterWorkSettings workSettings)
    {
        this.sensor = sensor;
        woodcutterWorkSettings = workSettings;
    }

    public override void Enter()
    {
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer < TimeAwait)
            return;

        PickUpWood();

        if (woodcutterWorkSettings.CarryCapacity <= woodCutterFacade.CarriedWood)
            ChangeState<WoodcutterReturnState>();
        else
            ChangeState<WoodcutterSearchTreeState>();
    }

    private void PickUpWood()
    {
        foreach (var drop in sensor.GetDropsInRadius(15))
        {
            drop.PickUp(view.transform, CollectStrategyType.NPC, 0);
            woodCutterFacade.AddWood(1);
        }
    }

    public override void Exit()
    {
        timer = 0f;
    }
}
