using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public sealed class SawmillCounterAnimator : IDisposable
{
    private readonly SawmillView view;
    private readonly SawmillCounterFeedbackSettings settings;

    private Sequence uiSequence;
    private Color baseColor = Color.white;
    private bool baseColorCached;

    public SawmillCounterAnimator(SawmillView view, SawmillCounterFeedbackSettings settings)
    {
        this.view = view;
        this.settings = settings;
    }

    public void Dispose()
    {
        uiSequence?.Kill();

        TMP_Text counterText = view.CurrentWoodText;
        if (counterText == null)
            return;

        counterText.rectTransform.localScale = Vector3.one;
        if (baseColorCached)
            counterText.color = baseColor;
    }

    public void PlayStorageChanged()
    {
        TMP_Text counterText = view.CurrentWoodText;
        if (counterText == null)
            return;

        uiSequence?.Kill();
        CacheBaseColor(counterText);
        counterText.rectTransform.localScale = Vector3.one;
        counterText.color = baseColor;

        uiSequence = DOTween.Sequence();
        uiSequence.Join(
            counterText.rectTransform.DOPunchScale(
                Vector3.one * settings.ScalePunch,
                settings.ScalePunchDuration,
                vibrato: 1,
                elasticity: 0f));

        if (settings.FlashDuration <= 0f)
            return;

        uiSequence.Join(DOTween.To(
                () => counterText.color,
                color => counterText.color = color,
                settings.FlashColor,
                settings.FlashDuration * 0.5f)
            .SetLoops(2, LoopType.Yoyo));
    }

    private void CacheBaseColor(TMP_Text counterText)
    {
        if (baseColorCached)
            return;

        baseColor = counterText.color;
        baseColorCached = true;
    }
}
