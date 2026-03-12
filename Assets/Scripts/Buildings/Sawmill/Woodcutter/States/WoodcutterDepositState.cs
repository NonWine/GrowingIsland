using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WoodcutterDepositState : WoodcutterState
{
    private Coroutine _depositRoutine;
    private GameObject _heldLogInstance;
    private Vector3 _visualRootBasePosition;
    private Vector3 _visualRootBaseEuler;
    private bool _isActive;
    private int _throwIndex;
    private bool _startWithVariantB;

    public override void Enter()
    {
        _isActive = true;
        _throwIndex = 0;
        _startWithVariantB = Random.value > 0.5f;

        view.Agent.isStopped = true;
        CacheVisualRootPose();
        RefreshHeldLogVisual();

        _depositRoutine = view.StartCoroutine(DepositRoutine());
    }

    public override void Tick() { }

    public override void Exit()
    {
        _isActive = false;

        if (_depositRoutine != null)
            view.StopCoroutine(_depositRoutine);

        view.transform.DOKill();
        view.VisualRoot.DOKill();
        DestroyHeldLogVisual();
        ResetVisualPose();
    }

    private IEnumerator DepositRoutine()
    {
        yield return RotateTowardsTarget();

        while (_isActive && woodCutterFacade.HasWood && !woodCutterFacade.WorkPlaceStorageFull)
        {
            ThrowExecutionPlan plan = BuildThrowPlan(_throwIndex++);
            RefreshHeldLogVisual();

            if (_heldLogInstance == null)
                break;

            yield return RotateTowardsTarget();
            yield return PlayThrow(plan);

            if (!_isActive)
                yield break;

            if (plan.PostThrowDelay > 0f)
                yield return new WaitForSeconds(plan.PostThrowDelay);
        }

        if (_isActive)
            ChangeNext();
    }

    private IEnumerator PlayThrow(ThrowExecutionPlan plan)
    {
        yield return AnimatePose(
            plan.BodyAnticipationOffset,
            plan.BodyAnticipationEuler,
            plan.HeldAnticipationPosition,
            plan.HeldRotationEuler,
            plan.AnticipationDuration,
            Ease.OutSine);

        bool impactResolved = false;
        yield return AnimatePose(
            plan.BodyReleaseOffset,
            plan.BodyReleaseEuler,
            plan.HeldReleasePosition,
            plan.HeldRotationEuler,
            plan.ReleaseDuration,
            Ease.OutCubic,
            onComplete: () => ReleaseHeldLog(plan, () => impactResolved = true));

        while (_isActive && !impactResolved)
            yield return null;

        if (!_isActive)
            yield break;

        yield return AnimatePose(
            Vector3.zero,
            Vector3.zero,
            workSettings.DepositAnimation.HeldLogLocalPosition,
            workSettings.DepositAnimation.HeldLogLocalEuler,
            plan.RecoveryDuration,
            Ease.OutQuad);
    }

    private IEnumerator RotateTowardsTarget()
    {
        Vector3 direction = GetTargetDirection();
        if (direction.sqrMagnitude <= 0.0001f)
            yield break;

        view.transform.DOKill();
        Tween rotateTween = view.transform
            .DORotateQuaternion(Quaternion.LookRotation(direction), workSettings.DepositAnimation.RotationDuration)
            .SetEase(Ease.OutSine);

        yield return rotateTween.WaitForCompletion();
    }

    private IEnumerator AnimatePose(
        Vector3 bodyOffset,
        Vector3 bodyEuler,
        Vector3 heldLocalPosition,
        Vector3 heldLocalEuler,
        float duration,
        Ease ease,
        TweenCallback onComplete = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(
            view.VisualRoot.DOLocalMove(_visualRootBasePosition + bodyOffset, duration).SetEase(ease));
        sequence.Join(
            view.VisualRoot.DOLocalRotate(_visualRootBaseEuler + bodyEuler, duration).SetEase(ease));

        if (_heldLogInstance != null)
        {
            sequence.Join(_heldLogInstance.transform.DOLocalMove(heldLocalPosition, duration).SetEase(ease));
            sequence.Join(_heldLogInstance.transform.DOLocalRotate(heldLocalEuler, duration).SetEase(ease));
        }

        if (onComplete != null)
            sequence.OnComplete(onComplete);

        yield return sequence.WaitForCompletion();
    }

    private void ReleaseHeldLog(ThrowExecutionPlan plan, TweenCallback onImpact)
    {
        Vector3 startPosition = view.ThrowOrigin.position;
        Vector3 startEuler = view.ThrowOrigin.rotation.eulerAngles;

        if (_heldLogInstance != null)
        {
            startPosition = _heldLogInstance.transform.position;
            startEuler = _heldLogInstance.transform.rotation.eulerAngles;
            Object.Destroy(_heldLogInstance);
            _heldLogInstance = null;
        }

        PlayReleaseSound();
        SpawnProjectile(startPosition, startEuler, plan, onImpact);
    }

    private void SpawnProjectile(Vector3 startPosition, Vector3 startEuler, ThrowExecutionPlan plan, TweenCallback onImpact)
    {
        var settings = workSettings.DepositAnimation;
        if (settings.LogPrefab == null)
        {
            woodCutterFacade.DepositOneWood(plan.ImpactStrength);
            onImpact?.Invoke();
            return;
        }

        Vector3 endPosition = woodCutterFacade.DepositPoint != null
            ? woodCutterFacade.DepositPoint.position + plan.LandingOffset
            : woodCutterFacade.WorkPlacePosition + plan.LandingOffset;

        GameObject projectile = Object.Instantiate(settings.LogPrefab, startPosition, Quaternion.Euler(startEuler));
        DisablePhysics(projectile);

        Transform projectileTransform = projectile.transform;
        Vector3 finalScale = projectileTransform.localScale * settings.ProjectileScale;
        projectileTransform.localScale = finalScale * 0.92f;

        Vector3 direction = endPosition - startPosition;
        Vector3 sideAxis = Vector3.Cross(Vector3.up, direction.normalized);
        if (sideAxis.sqrMagnitude <= 0.0001f)
            sideAxis = view.transform.right;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(
            DOTween.To(
                    () => 0f,
                    progress =>
                    {
                        float easedProgress = DOVirtual.EasedValue(0f, 1f, progress, settings.LogFlightEase);
                        Vector3 linearPosition = Vector3.LerpUnclamped(startPosition, endPosition, easedProgress);
                        float arcHeight = Mathf.Sin(progress * Mathf.PI) * plan.ArcHeight;
                        float sideOffset = Mathf.Sin(progress * Mathf.PI) * plan.SideOffset;
                        projectileTransform.position = linearPosition + Vector3.up * arcHeight + sideAxis * sideOffset;
                    },
                    1f,
                    plan.FlightDuration)
                .SetEase(Ease.Linear));

        sequence.Join(projectileTransform.DOScale(finalScale, Mathf.Min(0.08f, plan.FlightDuration * 0.5f)).SetEase(Ease.OutSine));
        sequence.Join(
            projectileTransform.DORotate(
                startEuler + new Vector3(plan.PitchSpin, plan.SpinDegrees, plan.RollSpin),
                plan.FlightDuration,
                RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

        sequence.OnComplete(() =>
        {
            Object.Destroy(projectile);
            woodCutterFacade.DepositOneWood(plan.ImpactStrength);
            onImpact?.Invoke();
        });
    }

    private void RefreshHeldLogVisual()
    {
        if (!woodCutterFacade.HasWood || workSettings.DepositAnimation.LogPrefab == null)
        {
            DestroyHeldLogVisual();
            return;
        }

        if (_heldLogInstance != null)
            return;

        var settings = workSettings.DepositAnimation;
        _heldLogInstance = Object.Instantiate(settings.LogPrefab, view.HeldLogAnchor);
        DisablePhysics(_heldLogInstance);
        _heldLogInstance.transform.localPosition = settings.HeldLogLocalPosition;
        _heldLogInstance.transform.localEulerAngles = settings.HeldLogLocalEuler;
        _heldLogInstance.transform.localScale *= settings.ProjectileScale;
    }

    private void DestroyHeldLogVisual()
    {
        if (_heldLogInstance == null)
            return;

        Object.Destroy(_heldLogInstance);
        _heldLogInstance = null;
    }

    private void CacheVisualRootPose()
    {
        _visualRootBasePosition = view.VisualRoot.localPosition;
        _visualRootBaseEuler = view.VisualRoot.localEulerAngles;
    }

    private void ResetVisualPose()
    {
        view.VisualRoot.localPosition = _visualRootBasePosition;
        view.VisualRoot.localEulerAngles = _visualRootBaseEuler;
    }

    private ThrowExecutionPlan BuildThrowPlan(int throwNumber)
    {
        var settings = workSettings.DepositAnimation;
        ThrowVariantSettings variant = SelectVariant(settings, throwNumber);
        float timingMultiplier = throwNumber == 0
            ? settings.FirstThrowTimingMultiplier
            : settings.ComboTimingMultiplier;

        timingMultiplier *= Mathf.Max(0.1f, variant.TimingMultiplier);

        bool isAccented = settings.AccentEvery > 0 && (throwNumber + 1) % settings.AccentEvery == 0;
        float accentMultiplier = isAccented ? settings.AccentImpactMultiplier : 1f;

        return new ThrowExecutionPlan
        {
            VariantName = variant.Name,
            AnticipationDuration = ReadRange(settings.AnticipationDurationRange) * timingMultiplier,
            ReleaseDuration = ReadRange(settings.ReleaseDurationRange) * timingMultiplier,
            FlightDuration = ReadRange(settings.FlightDurationRange),
            RecoveryDuration = ReadRange(settings.RecoveryDurationRange) * timingMultiplier,
            PostThrowDelay = Mathf.Max(0f, settings.DelayBetweenLogs),
            ArcHeight = ReadRange(settings.ArcHeightRange) * variant.ArcHeightMultiplier * (isAccented ? 1.05f : 1f),
            SideOffset = ReadRange(settings.SideOffsetRange),
            SpinDegrees = ReadRange(settings.SpinDegreesRange) * variant.SpinMultiplier,
            PitchSpin = ReadRange(settings.PitchJitterRange),
            RollSpin = ReadRange(settings.RollJitterRange),
            ImpactStrength = ReadRange(settings.ImpactStrengthRange) * accentMultiplier,
            BodyAnticipationOffset = Vector3.Scale(settings.BodyAnticipationOffset, variant.BodyOffsetScale),
            BodyAnticipationEuler = settings.BodyAnticipationEuler,
            BodyReleaseOffset = Vector3.Scale(settings.BodyReleaseOffset, variant.BodyOffsetScale) * accentMultiplier,
            BodyReleaseEuler = settings.BodyReleaseEuler + variant.BodyReleaseEulerOffset,
            HeldAnticipationPosition = settings.HeldLogLocalPosition + variant.HeldAnticipationOffset,
            HeldReleasePosition = settings.HeldLogLocalPosition + variant.HeldReleaseOffset,
            HeldRotationEuler = settings.HeldLogLocalEuler,
            LandingOffset = new Vector3(
                ReadRange(settings.LandingSpreadRange),
                0f,
                ReadRange(settings.LandingSpreadRange))
        };
    }

    private ThrowVariantSettings SelectVariant(DepositAnimationSettings settings, int throwNumber)
    {
        bool useVariantB = (throwNumber + (_startWithVariantB ? 1 : 0)) % 2 == 1;
        return useVariantB ? settings.ThrowB : settings.ThrowA;
    }

    private Vector3 GetTargetDirection()
    {
        Vector3 targetPosition = woodCutterFacade.DepositPoint != null
            ? woodCutterFacade.DepositPoint.position
            : woodCutterFacade.WorkPlacePosition;

        Vector3 direction = targetPosition - view.transform.position;
        direction.y = 0f;
        return direction.normalized;
    }

    private void PlayReleaseSound()
    {
        var settings = workSettings.DepositAnimation;
        AudioClip clip = PickRandom(settings.ReleaseClips);
        if (clip == null)
            return;

        if (view.AudioSource != null)
        {
            view.AudioSource.pitch = ReadRange(settings.ReleaseSoundPitchRange);
            view.AudioSource.PlayOneShot(clip, settings.ReleaseVolume);
            return;
        }

        AudioSource.PlayClipAtPoint(clip, view.transform.position, settings.ReleaseVolume);
    }

    private static AudioClip PickRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        return clips[Random.Range(0, clips.Length)];
    }

    private static float ReadRange(Vector2 range)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);
        return Random.Range(min, max);
    }

    private static void DisablePhysics(GameObject visual)
    {
        foreach (Collider collider in visual.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in visual.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }

    private void ChangeNext()
    {
        if (woodCutterFacade.WorkPlaceStorageFull)
            ChangeState<WoodcutterWaitingState>();
        else
            ChangeState<WoodcutterSearchTreeState>();
    }

    private struct ThrowExecutionPlan
    {
        public string VariantName;
        public float AnticipationDuration;
        public float ReleaseDuration;
        public float FlightDuration;
        public float RecoveryDuration;
        public float PostThrowDelay;
        public float ArcHeight;
        public float SideOffset;
        public float SpinDegrees;
        public float PitchSpin;
        public float RollSpin;
        public float ImpactStrength;
        public Vector3 BodyAnticipationOffset;
        public Vector3 BodyAnticipationEuler;
        public Vector3 BodyReleaseOffset;
        public Vector3 BodyReleaseEuler;
        public Vector3 HeldAnticipationPosition;
        public Vector3 HeldReleasePosition;
        public Vector3 HeldRotationEuler;
        public Vector3 LandingOffset;
    }
}
