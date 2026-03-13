using System;
using System.Collections;
using UnityEngine;

public sealed class WoodcutterDepositRoutine : IWoodcutterDepositRoutine
{
    private readonly WoodCutterFacade _woodCutterFacade;
    private readonly WoodcutterWorkSettings _workSettings;
    private readonly IWoodcutterDepositPlanBuilder _planBuilder;
    private readonly IWoodcutterDepositVisualController _visualController;
    private readonly IWoodcutterDepositThrowSequence _throwSequence;

    public WoodcutterDepositRoutine(
        WoodCutterFacade woodCutterFacade,
        WoodcutterWorkSettings workSettings,
        IWoodcutterDepositPlanBuilder planBuilder,
        IWoodcutterDepositVisualController visualController,
        IWoodcutterDepositThrowSequence throwSequence)
    {
        _woodCutterFacade = woodCutterFacade;
        _workSettings = workSettings;
        _planBuilder = planBuilder;
        _visualController = visualController;
        _throwSequence = throwSequence;
    }

    public IEnumerator Execute(bool startWithVariantB, Func<bool> isActive, Action<WoodcutterDepositRoutineResult> onCompleted)
    {
        int throwIndex = 0;

        yield return _visualController.RotateTowards(GetTargetPosition());

        while (isActive() && _woodCutterFacade.HasWood && !_woodCutterFacade.WorkPlaceStorageFull)
        {
            WoodcutterDepositThrowPlan plan = _planBuilder.Build(
                throwIndex++,
                startWithVariantB,
                _workSettings.DepositAnimation);

            _visualController.RefreshHeldLog(_woodCutterFacade.HasWood);
            if (!_visualController.HasHeldLog)
                break;

            yield return _visualController.RotateTowards(GetTargetPosition());
            yield return _throwSequence.Execute(plan, isActive);

            if (!isActive())
            {
                onCompleted?.Invoke(WoodcutterDepositRoutineResult.Interrupted);
                yield break;
            }

            if (plan.PostThrowDelay > 0f)
                yield return new WaitForSeconds(plan.PostThrowDelay);
        }

        if (!isActive())
        {
            onCompleted?.Invoke(WoodcutterDepositRoutineResult.Interrupted);
            yield break;
        }

        onCompleted?.Invoke(
            _woodCutterFacade.WorkPlaceStorageFull
                ? WoodcutterDepositRoutineResult.WaitForStorage
                : WoodcutterDepositRoutineResult.ContinueSearching);
    }

    private Vector3 GetTargetPosition()
    {
        return _woodCutterFacade.DepositPoint != null
            ? _woodCutterFacade.DepositPoint.position
            : _woodCutterFacade.WorkPlacePosition;
    }
}
