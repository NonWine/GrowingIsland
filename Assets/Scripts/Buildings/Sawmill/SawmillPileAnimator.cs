using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public sealed class SawmillPileAnimator : ISawmillPileAnimator
{
    public void ApplyVisibility(IReadOnlyList<Transform> stageRoots, SawmillPileVisualSettings settings, int visibleStages, bool animateStageChange)
    {
        for (int i = 0; i < stageRoots.Count; i++)
        {
            Transform stageRoot = stageRoots[i];
            if (stageRoot == null)
                continue;

            bool shouldBeVisible = i < visibleStages;
            if (!shouldBeVisible)
            {
                stageRoot.DOKill();
                stageRoot.localScale = Vector3.one;
                stageRoot.gameObject.SetActive(false);
                continue;
            }

            if (stageRoot.gameObject.activeSelf)
                continue;

            stageRoot.gameObject.SetActive(true);
            stageRoot.localScale = animateStageChange ? Vector3.zero : Vector3.one;

            if (!animateStageChange)
                continue;

            stageRoot.DOScale(1f + settings.StagePopScale, settings.StagePopDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    stageRoot.DOScale(1f, settings.StageSettleDuration).SetEase(Ease.InOutSine));
        }
    }

    public void PlayImpact(IReadOnlyList<Transform> stageRoots, SawmillImpactFeedbackSettings impactFeedback, float impactStrength)
    {
        if (stageRoots.Count == 0)
            return;

        float strength = Mathf.Max(0.2f, impactStrength);
        int shakenStages = 0;

        for (int i = stageRoots.Count - 1; i >= 0 && shakenStages < Mathf.Max(1, impactFeedback.MaxPileShakeStages); i--)
        {
            Transform stageRoot = stageRoots[i];
            if (stageRoot == null || !stageRoot.gameObject.activeSelf)
                continue;

            stageRoot.DOKill();
            stageRoot.DOPunchPosition(Vector3.up * (impactFeedback.PileSettlePunch * strength), impactFeedback.Duration * 0.85f, 1, 0f);
            stageRoot.DOPunchRotation(new Vector3(0f, impactFeedback.PileRotationPunch * strength, 0f), impactFeedback.Duration * 0.85f, 1, 0f);
            shakenStages++;
        }
    }
}
