using System.Collections;
using DG.Tweening;
using UnityEngine;

public sealed class WoodcutterDepositVisualController : IWoodcutterDepositVisualController
{
    private readonly WoodcutterView _view;
    private readonly WoodcutterWorkSettings _workSettings;

    private GameObject _heldLogInstance;
    private Vector3 _visualRootBasePosition;
    private Vector3 _visualRootBaseEuler;
    private Tween _rotateTween;
    private Sequence _poseSequence;

    public WoodcutterDepositVisualController(WoodcutterView view, WoodcutterWorkSettings workSettings)
    {
        _view = view;
        _workSettings = workSettings;
    }

    public bool HasHeldLog => _heldLogInstance != null;

    public void BeginSession(bool hasWood)
    {
        CacheVisualRootPose();
        RefreshHeldLog(hasWood);
    }

    public void EndSession()
    {
        _rotateTween?.Kill();
        _poseSequence?.Kill();
        DestroyHeldLogVisual();
        ResetVisualPose();
    }

    public void RefreshHeldLog(bool hasWood)
    {
        if (!hasWood || _workSettings.DepositAnimation.LogPrefab == null)
        {
            DestroyHeldLogVisual();
            return;
        }

        if (_heldLogInstance != null)
            return;

        DepositAnimationSettings settings = _workSettings.DepositAnimation;
        _heldLogInstance = Object.Instantiate(settings.LogPrefab, _view.HeldLogAnchor);
        DisablePhysics(_heldLogInstance);
        _heldLogInstance.transform.localPosition = settings.HeldLogLocalPosition;
        _heldLogInstance.transform.localEulerAngles = settings.HeldLogLocalEuler;
        _heldLogInstance.transform.localScale *= settings.ProjectileScale;
    }

    public WoodcutterReleasedLogData ReleaseHeldLog()
    {
        Vector3 startPosition = _view.ThrowOrigin.position;
        Vector3 startEuler = _view.ThrowOrigin.rotation.eulerAngles;

        if (_heldLogInstance == null)
            return new WoodcutterReleasedLogData(startPosition, startEuler);

        startPosition = _heldLogInstance.transform.position;
        startEuler = _heldLogInstance.transform.rotation.eulerAngles;
        Object.Destroy(_heldLogInstance);
        _heldLogInstance = null;

        return new WoodcutterReleasedLogData(startPosition, startEuler);
    }

    public IEnumerator RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - _view.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
            yield break;

        _rotateTween?.Kill();
        _rotateTween = _view.transform
            .DORotateQuaternion(Quaternion.LookRotation(direction.normalized), _workSettings.DepositAnimation.RotationDuration)
            .SetEase(Ease.OutSine);

        yield return _rotateTween.WaitForCompletion();
    }

    public IEnumerator AnimatePose(WoodcutterDepositPose pose, float duration, Ease ease, TweenCallback onComplete = null)
    {
        _poseSequence?.Kill();
        _poseSequence = DOTween.Sequence();
        _poseSequence.Join(
            _view.VisualRoot.DOLocalMove(_visualRootBasePosition + pose.BodyOffset, duration).SetEase(ease));
        _poseSequence.Join(
            _view.VisualRoot.DOLocalRotate(_visualRootBaseEuler + pose.BodyEuler, duration).SetEase(ease));

        if (_heldLogInstance != null)
        {
            _poseSequence.Join(_heldLogInstance.transform.DOLocalMove(pose.HeldLocalPosition, duration).SetEase(ease));
            _poseSequence.Join(_heldLogInstance.transform.DOLocalRotate(pose.HeldLocalEuler, duration).SetEase(ease));
        }

        if (onComplete != null)
            _poseSequence.OnComplete(onComplete);

        yield return _poseSequence.WaitForCompletion();
    }

    private void CacheVisualRootPose()
    {
        _visualRootBasePosition = _view.VisualRoot.localPosition;
        _visualRootBaseEuler = _view.VisualRoot.localEulerAngles;
    }

    private void ResetVisualPose()
    {
        _view.VisualRoot.localPosition = _visualRootBasePosition;
        _view.VisualRoot.localEulerAngles = _visualRootBaseEuler;
    }

    private void DestroyHeldLogVisual()
    {
        if (_heldLogInstance == null)
            return;

        Object.Destroy(_heldLogInstance);
        _heldLogInstance = null;
    }

    private static void DisablePhysics(GameObject visual)
    {
        foreach (Collider collider in visual.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in visual.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }
}
