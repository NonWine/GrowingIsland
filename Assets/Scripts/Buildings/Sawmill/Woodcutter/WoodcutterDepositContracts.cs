using System.Collections;
using System;
using DG.Tweening;
public interface IWoodcutterDepositPlanBuilder
{
    WoodcutterDepositThrowPlan Build(int throwNumber, bool startWithVariantB, DepositAnimationSettings settings);
}

public interface IWoodcutterDepositProjectileLauncher
{
    void Launch(WoodcutterReleasedLogData releaseData, WoodcutterDepositThrowPlan plan, TweenCallback onImpactResolved);
    void ResetSession();
}

public interface IWoodcutterDepositThrowSequence
{
    IEnumerator Execute(WoodcutterDepositThrowPlan plan, UnityEngine.Vector3 targetPosition, Func<bool> isActive, Action<float> onImpact);
}

public interface IWoodcutterDepositRoutine
{
    IEnumerator Execute(bool startWithVariantB, Func<bool> isActive, Action<WoodcutterDepositRoutineResult> onCompleted);
}

public interface IWoodcutterDepositSession
{
    void Start(Action<WoodcutterDepositRoutineResult> onCompleted);
    void Stop();
}
