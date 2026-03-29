using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class WoodcutterDepositThrowSequence
{
    private readonly WoodcutterDepositVisualController visualController;
    private readonly WoodcutterDepositProjectileLauncher projectileLauncher;
    private readonly WoodcutterWorkSettings workSettings;

    public WoodcutterDepositThrowSequence(
        WoodcutterDepositVisualController visualController,
        WoodcutterDepositProjectileLauncher projectileLauncher,
        WoodcutterWorkSettings workSettings)
    {
        this.visualController = visualController;
        this.projectileLauncher = projectileLauncher;
        this.workSettings = workSettings;
    }

    public IEnumerator Execute(WoodcutterDepositThrowPlan plan, Vector3 targetPosition, Func<bool> isActive, Action<float> onImpact)
    {
        yield return visualController.AnimatePose(
            plan.AnticipationPose,
            plan.AnticipationDuration,
            Ease.OutSine);

        bool impactResolved = false;
        yield return visualController.AnimatePose(
            plan.ReleasePose,
            plan.ReleaseDuration,
            Ease.OutCubic,
            () => projectileLauncher.Launch(
                visualController.ReleaseHeldLog(targetPosition),
                plan,
                () =>
                {
                    impactResolved = true;
                    onImpact?.Invoke(plan.ImpactStrength);
                }));

        while (isActive() && !impactResolved)
            yield return null;

        if (!isActive())
            yield break;

        yield return visualController.AnimatePose(
            plan.CreateRecoveryPose(workSettings.DepositAnimation),
            plan.RecoveryDuration,
            Ease.OutQuad);
    }
}
