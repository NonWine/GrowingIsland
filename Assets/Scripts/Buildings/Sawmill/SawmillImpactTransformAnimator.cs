using System;
using DG.Tweening;
using UnityEngine;

public sealed class SawmillImpactTransformAnimator : ISawmillImpactAnimator, IDisposable
{
    private readonly ISawmillImpactFeedbackView _view;

    private Sequence _impactSequence;
    private Vector3 _baseLocalPosition;
    private Quaternion _baseLocalRotation;
    private Vector3 _baseLocalScale;
    private bool _basePoseCaptured;

    public SawmillImpactTransformAnimator(ISawmillImpactFeedbackView view)
    {
        _view = view;
    }

    public void Dispose()
    {
        _impactSequence?.Kill();
        ResetPose();
    }

    public void Play(float impactStrength)
    {
        CacheBasePose();
        _impactSequence?.Kill();
        ResetPose();

        Transform impactRoot = _view.ImpactRoot;
        SawmillImpactFeedbackSettings feedback = _view.ImpactFeedbackSettings;
        float strength = Mathf.Max(0.2f, impactStrength);

        _impactSequence = DOTween.Sequence();
        _impactSequence.Join(
            impactRoot.DOPunchScale(
                Vector3.one * (feedback.ScalePunch * strength),
                feedback.Duration,
                vibrato: 1,
                elasticity: 0f));

        if (feedback.PositionShake > 0f)
        {
            _impactSequence.Join(
                impactRoot.DOShakePosition(
                    feedback.Duration,
                    feedback.PositionShake * strength,
                    feedback.ShakeVibrato,
                    randomness: 90f,
                    snapping: false,
                    fadeOut: true));
        }

        if (feedback.RotationPunch > 0f)
        {
            _impactSequence.Join(
                impactRoot.DOPunchRotation(
                    new Vector3(0f, 0f, -feedback.RotationPunch * strength),
                    feedback.Duration,
                    vibrato: 1,
                    elasticity: 0f));
        }
    }

    private void CacheBasePose()
    {
        if (_basePoseCaptured)
            return;

        Transform impactRoot = _view.ImpactRoot;
        _baseLocalPosition = impactRoot.localPosition;
        _baseLocalRotation = impactRoot.localRotation;
        _baseLocalScale = impactRoot.localScale;
        _basePoseCaptured = true;
    }

    private void ResetPose()
    {
        if (!_basePoseCaptured)
            return;

        Transform impactRoot = _view.ImpactRoot;
        impactRoot.localPosition = _baseLocalPosition;
        impactRoot.localRotation = _baseLocalRotation;
        impactRoot.localScale = _baseLocalScale;
    }
}
