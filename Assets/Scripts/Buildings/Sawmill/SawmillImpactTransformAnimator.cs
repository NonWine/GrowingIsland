using System;
using DG.Tweening;
using UnityEngine;

public sealed class SawmillImpactTransformAnimator : IDisposable
{
    private readonly SawmillView view;
    private readonly SawmillImpactFeedbackSettings settings;

    private Sequence impactSequence;
    private Vector3 baseLocalPosition;
    private Quaternion baseLocalRotation;
    private Vector3 baseLocalScale;
    private bool basePoseCaptured;

    public SawmillImpactTransformAnimator(SawmillView view, SawmillImpactFeedbackSettings settings)
    {
        this.view = view;
        this.settings = settings;
    }

    public void Dispose()
    {
        impactSequence?.Kill();
        ResetPose();
    }

    public void Play(float impactStrength)
    {
        CacheBasePose();
        impactSequence?.Kill();
        ResetPose();

        Transform impactRoot = view.ImpactRoot;
        float strength = Mathf.Max(0.2f, impactStrength);

        impactSequence = DOTween.Sequence();
        impactSequence.Join(
            impactRoot.DOPunchScale(
                Vector3.one * (settings.ScalePunch * strength),
                settings.Duration,
                vibrato: 1,
                elasticity: 0f));

        if (settings.PositionShake > 0f)
        {
            impactSequence.Join(
                impactRoot.DOShakePosition(
                    settings.Duration,
                    settings.PositionShake * strength,
                    settings.ShakeVibrato,
                    randomness: 90f,
                    snapping: false,
                    fadeOut: true));
        }

        if (settings.RotationPunch > 0f)
        {
            impactSequence.Join(
                impactRoot.DOPunchRotation(
                    new Vector3(0f, 0f, -settings.RotationPunch * strength),
                    settings.Duration,
                    vibrato: 1,
                    elasticity: 0f));
        }
    }

    private void CacheBasePose()
    {
        if (basePoseCaptured)
            return;

        Transform impactRoot = view.ImpactRoot;
        baseLocalPosition = impactRoot.localPosition;
        baseLocalRotation = impactRoot.localRotation;
        baseLocalScale = impactRoot.localScale;
        basePoseCaptured = true;
    }

    private void ResetPose()
    {
        if (!basePoseCaptured)
            return;

        Transform impactRoot = view.ImpactRoot;
        impactRoot.localPosition = baseLocalPosition;
        impactRoot.localRotation = baseLocalRotation;
        impactRoot.localScale = baseLocalScale;
    }
}
