using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class WoodcutterDepositVisualController : IWoodcutterDepositVisualController
{
    private readonly WoodcutterView view;
    private readonly WoodcutterWorkSettings workSettings;

    private GameObject heldLogInstance;
    private Tween rotateTween;
    private Sequence poseSequence;

    public WoodcutterDepositVisualController(WoodcutterView view, WoodcutterWorkSettings workSettings)
    {
        this.view = view;
        this.workSettings = workSettings;
    }

    public bool HasHeldLog => heldLogInstance != null;

    public void BeginSession(bool hasWood)
    {
        RefreshHeldLog(hasWood);
    }

    public void EndSession()
    {
        rotateTween?.Kill();
        poseSequence?.Kill();
        DestroyHeldLogVisual();
    }

    public void RefreshHeldLog(bool hasWood)
    {
        if (!hasWood || workSettings.DepositAnimation.LogPrefab == null)
        {
            DestroyHeldLogVisual();
            return;
        }

        if (heldLogInstance != null)
            return;

        DepositAnimationSettings settings = workSettings.DepositAnimation;
        heldLogInstance = Object.Instantiate(settings.LogPrefab, view.HeldLogAnchor);
        DisablePhysics(heldLogInstance);
        heldLogInstance.transform.localPosition = settings.HeldLogLocalPosition;
        heldLogInstance.transform.localEulerAngles = settings.HeldLogLocalEuler;
        heldLogInstance.transform.localScale *= settings.ProjectileScale;
    }

    public WoodcutterReleasedLogData ReleaseHeldLog(Vector3 targetPosition)
    {
        Vector3 startPosition = view.ThrowOrigin.position;
        Vector3 startEuler = view.ThrowOrigin.rotation.eulerAngles;

        if (heldLogInstance == null)
            return new WoodcutterReleasedLogData(startPosition, startEuler, targetPosition);

        startPosition = heldLogInstance.transform.position;
        startEuler = heldLogInstance.transform.rotation.eulerAngles;
        Object.Destroy(heldLogInstance);
        heldLogInstance = null;

        return new WoodcutterReleasedLogData(startPosition, startEuler, targetPosition);
    }

    public IEnumerator RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - view.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
            yield break;

        rotateTween?.Kill();
        rotateTween = view.transform
            .DORotateQuaternion(Quaternion.LookRotation(direction.normalized), workSettings.DepositAnimation.RotationDuration)
            .SetEase(Ease.OutSine);

        yield return rotateTween.WaitForCompletion();
    }

    public IEnumerator AnimatePose(WoodcutterDepositPose pose, float duration, Ease ease, TweenCallback onComplete = null)
    {
        poseSequence?.Kill();
        poseSequence = DOTween.Sequence();

        if (heldLogInstance != null)
        {
            poseSequence.Join(heldLogInstance.transform.DOLocalMove(pose.HeldLocalPosition, duration).SetEase(ease));
            poseSequence.Join(heldLogInstance.transform.DOLocalRotate(pose.HeldLocalEuler, duration).SetEase(ease));
        }
        else
        {
            poseSequence.AppendInterval(duration);
        }

        if (onComplete != null)
            poseSequence.OnComplete(onComplete);

        yield return poseSequence.WaitForCompletion();
    }

    private void DestroyHeldLogVisual()
    {
        if (heldLogInstance == null)
            return;

        Object.Destroy(heldLogInstance);
        heldLogInstance = null;
    }

    private static void DisablePhysics(GameObject visual)
    {
        foreach (Collider collider in visual.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in visual.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }
}
