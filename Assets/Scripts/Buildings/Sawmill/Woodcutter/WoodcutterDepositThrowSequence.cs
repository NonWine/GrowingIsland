using System;
using System.Collections;
using DG.Tweening;

public sealed class WoodcutterDepositThrowSequence : IWoodcutterDepositThrowSequence
{
    private readonly IWoodcutterDepositVisualController _visualController;
    private readonly IWoodcutterDepositProjectileLauncher _projectileLauncher;
    private readonly WoodcutterWorkSettings _workSettings;

    public WoodcutterDepositThrowSequence(
        IWoodcutterDepositVisualController visualController,
        IWoodcutterDepositProjectileLauncher projectileLauncher,
        WoodcutterWorkSettings workSettings)
    {
        _visualController = visualController;
        _projectileLauncher = projectileLauncher;
        _workSettings = workSettings;
    }

    public IEnumerator Execute(WoodcutterDepositThrowPlan plan, Func<bool> isActive)
    {
        yield return _visualController.AnimatePose(
            plan.AnticipationPose,
            plan.AnticipationDuration,
            Ease.OutSine);

        bool impactResolved = false;
        yield return _visualController.AnimatePose(
            plan.ReleasePose,
            plan.ReleaseDuration,
            Ease.OutCubic,
            () => _projectileLauncher.Launch(
                _visualController.ReleaseHeldLog(),
                plan,
                () => impactResolved = true));

        while (isActive() && !impactResolved)
            yield return null;

        if (!isActive())
            yield break;

        yield return _visualController.AnimatePose(
            plan.CreateRecoveryPose(_workSettings.DepositAnimation),
            plan.RecoveryDuration,
            Ease.OutQuad);
    }
}
