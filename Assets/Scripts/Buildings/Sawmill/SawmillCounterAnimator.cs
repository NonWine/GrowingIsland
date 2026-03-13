using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public sealed class SawmillCounterAnimator : ISawmillStorageFeedback, IDisposable
{
    private readonly ISawmillCounterFeedbackView _view;

    private Sequence _uiSequence;
    private Color _baseColor = Color.white;
    private bool _baseColorCached;

    public SawmillCounterAnimator(ISawmillCounterFeedbackView view)
    {
        _view = view;
    }

    public void Dispose()
    {
        _uiSequence?.Kill();

        TMP_Text counterText = _view.CurrentWoodText;
        if (counterText == null)
            return;

        counterText.rectTransform.localScale = Vector3.one;
        if (_baseColorCached)
            counterText.color = _baseColor;
    }

    public void PlayStorageChanged()
    {
        TMP_Text counterText = _view.CurrentWoodText;
        if (counterText == null)
            return;

        _uiSequence?.Kill();
        CacheBaseColor(counterText);
        counterText.rectTransform.localScale = Vector3.one;
        counterText.color = _baseColor;

        SawmillCounterFeedbackSettings feedback = _view.CounterFeedbackSettings;

        _uiSequence = DOTween.Sequence();
        _uiSequence.Join(
            counterText.rectTransform.DOPunchScale(
                Vector3.one * feedback.ScalePunch,
                feedback.ScalePunchDuration,
                vibrato: 1,
                elasticity: 0f));

        if (feedback.FlashDuration <= 0f)
            return;

        _uiSequence.Join(DOTween.To(
                () => counterText.color,
                color => counterText.color = color,
                feedback.FlashColor,
                feedback.FlashDuration * 0.5f)
            .SetLoops(2, LoopType.Yoyo));
    }

    private void CacheBaseColor(TMP_Text counterText)
    {
        if (_baseColorCached)
            return;

        _baseColor = counterText.color;
        _baseColorCached = true;
    }
}
