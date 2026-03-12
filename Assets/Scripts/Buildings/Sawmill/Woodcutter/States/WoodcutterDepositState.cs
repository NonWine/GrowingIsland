using DG.Tweening;
using UnityEngine;

public class WoodcutterDepositState : WoodcutterState
{
    private Sequence _sequence;

    public override void Enter()
    {
        view.Agent.isStopped = true;

        var settings = workSettings.DepositAnimation;

        var dir = (woodCutterFacade.WorkPlacePosition - view.transform.position).normalized;
        dir.y = 0;

        _sequence = DOTween.Sequence();

        if (dir != Vector3.zero)
            _sequence.Append(view.transform.DORotateQuaternion(Quaternion.LookRotation(dir), settings.RotationDuration));

        int woodCount = woodCutterFacade.CarriedWood;
        for (int i = 0; i < woodCount; i++)
        {
            _sequence.AppendInterval(settings.DelayBetweenLogs);
            _sequence.AppendCallback(LaunchLog);
        }

        _sequence.OnComplete(ChangeNext);
    }

    private void LaunchLog()
    {
        var settings = workSettings.DepositAnimation;
        var spawnPos = view.transform.position + Vector3.up;
        var log = Object.Instantiate(settings.LogPrefab, spawnPos, Quaternion.identity);

        log.transform.DOMove(woodCutterFacade.WorkPlacePosition, settings.LogFlightDuration)
            .SetEase(settings.LogFlightEase)
            .OnComplete(() =>
            {
                Object.Destroy(log);
                woodCutterFacade.DepositOneWood();
            });
    }

    private void ChangeNext()
    {
        if (woodCutterFacade.WorkPlaceStorageFull)
            ChangeState<WoodcutterWaitingState>();
        else
            ChangeState<WoodcutterSearchTreeState>();
    }

    public override void Tick() { }

    public override void Exit()
    {
        _sequence?.Kill();
    }
}
