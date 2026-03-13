using System;
using System.Collections;
using UnityEngine;

public sealed class WoodcutterDepositRoutine : IWoodcutterDepositRoutine
{
    private readonly WoodCutterFacade woodCutterFacade;
    private readonly WoodcutterWorkSettings workSettings;
    private readonly IWoodcutterDepositPlanBuilder planBuilder;
    private readonly IWoodcutterDepositVisualController visualController;
    private readonly IWoodcutterDepositThrowSequence throwSequence;

    public WoodcutterDepositRoutine(
        WoodCutterFacade woodCutterFacade,
        WoodcutterWorkSettings workSettings,
        IWoodcutterDepositPlanBuilder planBuilder,
        IWoodcutterDepositVisualController visualController,
        IWoodcutterDepositThrowSequence throwSequence)
    {
        this.woodCutterFacade = woodCutterFacade;
        this.workSettings = workSettings;
        this.planBuilder = planBuilder;
        this.visualController = visualController;
        this.throwSequence = throwSequence;
    }

    public IEnumerator Execute(bool startWithVariantB, Func<bool> isActive, Action<WoodcutterDepositRoutineResult> onCompleted)
    {
        int throwIndex = 0;

        yield return visualController.RotateTowards(GetTargetPosition());

        while (isActive() && woodCutterFacade.HasWood && !woodCutterFacade.WorkPlaceStorageFull)
        {
            WoodcutterDepositThrowPlan plan = planBuilder.Build(throwIndex++, startWithVariantB, workSettings.DepositAnimation);
            Vector3 targetPosition = GetTargetPosition();

            visualController.RefreshHeldLog(woodCutterFacade.HasWood);
            if (!visualController.HasHeldLog)
                break;

            yield return visualController.RotateTowards(targetPosition);
            yield return throwSequence.Execute(
                plan,
                targetPosition,
                isActive,
                impactStrength => woodCutterFacade.DepositOneWood(impactStrength));

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

        onCompleted?.Invoke(woodCutterFacade.WorkPlaceStorageFull ? WoodcutterDepositRoutineResult.WaitForStorage : WoodcutterDepositRoutineResult.ContinueSearching);
    }

    private Vector3 GetTargetPosition()
    {
        return woodCutterFacade.DepositPoint != null
            ? woodCutterFacade.DepositPoint.position
            : woodCutterFacade.WorkPlacePosition;
    }
}
