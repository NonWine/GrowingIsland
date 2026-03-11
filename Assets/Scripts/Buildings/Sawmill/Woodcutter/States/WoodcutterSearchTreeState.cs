using UnityEngine;

public class WoodcutterSearchTreeState : WoodcutterState
{
    private readonly IWoodcutterSensor _sensor;
    private float _nextSearchTime;

    public WoodcutterSearchTreeState(IWoodcutterSensor sensor)
    {
        _sensor = sensor;
    }

    public override void Enter()
    {
        _nextSearchTime = 0f;
    }

    public override void Tick()
    {
        if (woodCutterFacade.StorageFull)
        {
            ChangeState<WoodcutterWaitingState>();
            return;
        }

        if (Time.time < _nextSearchTime)
            return;

        if (_sensor.TryFindNearest(out var tree))
        {
            woodCutterFacade.SetTree(tree);
            ChangeState<WoodcutterMoveToTreeState>();
        }
        else
        {
            _nextSearchTime = Time.time + Mathf.Max(0.05f, workSettings.RetargetCooldown);
        }
    }

    public override void Exit()
    {
    }
}
