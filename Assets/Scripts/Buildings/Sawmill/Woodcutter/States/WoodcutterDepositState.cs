using System.Collections;
using UnityEngine;
using Zenject;

public class WoodcutterDepositState : WoodcutterState
{
    [Inject] private IWoodcutterDepositVisualController _visualController;
    [Inject] private IWoodcutterDepositProjectileLauncher _projectileLauncher;
    [Inject] private IWoodcutterDepositRoutine _depositRoutineUseCase;

    private Coroutine _depositRoutine;
    private bool _isActive;
    private bool _startWithVariantB;

    public override void Enter()
    {
        _isActive = true;
        _startWithVariantB = Random.value > 0.5f;
        view.Agent.isStopped = true;
        _visualController.BeginSession(woodCutterFacade.HasWood);
        _depositRoutine = view.StartCoroutine(_depositRoutineUseCase.Execute(_startWithVariantB, () => _isActive, OnDepositRoutineCompleted));
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        _isActive = false;

        if (_depositRoutine != null)
            view.StopCoroutine(_depositRoutine);

        _projectileLauncher.ResetSession();
        _visualController.EndSession();
    }

    private void OnDepositRoutineCompleted(WoodcutterDepositRoutineResult result)
    {
        if (!_isActive)
            return;

        switch (result)
        {
            case WoodcutterDepositRoutineResult.WaitForStorage:
                ChangeState<WoodcutterWaitingState>();
                break;
            case WoodcutterDepositRoutineResult.ContinueSearching:
                ChangeState<WoodcutterSearchTreeState>();
                break;
        }
    }
}
