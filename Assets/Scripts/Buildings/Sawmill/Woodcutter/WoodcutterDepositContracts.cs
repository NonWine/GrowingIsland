using System.Collections;
using System;
using DG.Tweening;

public interface IWoodcutterDepositPlanBuilder
{
    WoodcutterDepositThrowPlan Build(int throwNumber, bool startWithVariantB, DepositAnimationSettings settings);
}

public interface IWoodcutterDepositVisualController
{
    bool HasHeldLog { get; }

    void BeginSession(bool hasWood);
    void EndSession();
    void RefreshHeldLog(bool hasWood);
    WoodcutterReleasedLogData ReleaseHeldLog();
    IEnumerator RotateTowards(UnityEngine.Vector3 targetPosition);
    IEnumerator AnimatePose(WoodcutterDepositPose pose, float duration, Ease ease, TweenCallback onComplete = null);
}

public interface IWoodcutterDepositProjectileLauncher
{
    void Launch(WoodcutterReleasedLogData releaseData, WoodcutterDepositThrowPlan plan, TweenCallback onImpact);
    void ResetSession();
}

public interface IWoodcutterDepositThrowSequence
{
    IEnumerator Execute(WoodcutterDepositThrowPlan plan, Func<bool> isActive);
}

public interface IWoodcutterDepositRoutine
{
    IEnumerator Execute(bool startWithVariantB, Func<bool> isActive, Action<WoodcutterDepositRoutineResult> onCompleted);
}
