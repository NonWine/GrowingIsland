using UnityEngine;

public class WoodcutterSearchTreeState : WoodcutterState
{
    private readonly IWoodcutterSensor sensor;
    private float nextSearchTime;

    public WoodcutterSearchTreeState(IWoodcutterSensor sensor)
    {
        this.sensor = sensor;
    }

    public override void Enter()
    {
        nextSearchTime = 0f;
    }

    public override void Tick()
    {
        if (woodCutterFacade.WorkPlaceStorageFull)
        {
            ChangeState<WoodcutterWaitingState>();
            return;
        }

        if (Time.time < nextSearchTime)
            return;

        if (sensor.TryFindNearest(out var tree))
        {
            woodCutterFacade.SetTree(tree);
            ChangeState<WoodcutterMoveToTreeState>();
        }
        else
        {
            nextSearchTime = Time.time + Mathf.Max(0.05f, workSettings.RetargetCooldown);
        }
    }

    public override void Exit()
    {
    }
}
