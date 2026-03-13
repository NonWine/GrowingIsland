using System.Collections;
using DG.Tweening;

public interface IWoodcutterDepositVisualController
{
    bool HasHeldLog { get; }

    void BeginSession(bool hasWood);
    void EndSession();
    void RefreshHeldLog(bool hasWood);
    WoodcutterReleasedLogData ReleaseHeldLog(UnityEngine.Vector3 targetPosition);
    IEnumerator RotateTowards(UnityEngine.Vector3 targetPosition);
    IEnumerator AnimatePose(WoodcutterDepositPose pose, float duration, Ease ease, TweenCallback onComplete = null);
}
