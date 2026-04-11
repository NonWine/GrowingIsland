
using System;
using DG.Tweening;
using UnityEngine;

public sealed class TreePreviewController : IDisposable
{
    private readonly EnvironmentPropObjectView view;
    private readonly TreeFinalFallSettings finalFallSettings;
    private readonly TreeResourceDropExecutor _resourceDropExecutor;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;
    private readonly TreeStumpController _stumpController;
    private IResetable resetable;

    private Sequence previewSequence;

    public TreePreviewController(
        EnvironmentPropObjectView view,
        TreeFinalFallSettings finalFallSettings,
        TreeResourceDropExecutor resourceDropExecutor,
        ITreeHitReaction hitReaction,
        ITreeFinalFallReaction finalFallReaction,
        TreeStumpController stumpController)
    {
        this.view = view;
        this.finalFallSettings = finalFallSettings;
        this._resourceDropExecutor = resourceDropExecutor;
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
        this._stumpController = stumpController;
    }

    public void Dispose()
    {
        KillPreviewSequence();
    }

    public void PreviewHit()
    {
        hitReaction.PlayHit(GetPreviewHitSource());
    }

    public void PreviewFinalFall()
    {
        finalFallReaction.Play(GetPreviewHitSource());
    }

    public void PreviewFullFinalHit()
    {
        KillPreviewSequence();
        finalFallReaction.Play(GetPreviewHitSource());
        previewSequence = DOTween.Sequence()
            .AppendInterval(GetFinalFallImpactDelay())
            .AppendCallback(HandlePreviewFinalFallImpact)
            .AppendInterval(finalFallSettings.LandImpactDuration)
            .AppendCallback(HandlePreviewFinalFallComplete)
            .SetLink(view.gameObject);
    }

    public void ResetPreview()
    {
        KillPreviewSequence();
        hitReaction.ResetToNeutral();
        finalFallReaction.ResetToNeutral();
        _stumpController.Hide();
        resetable.Reset();
        EnvironmentResourceViewUtility.SetChildrenVisible(view.transform, true);
    }

    private void HandlePreviewFinalFallImpact()
    {
        _resourceDropExecutor.Spawn();
    }

    private void HandlePreviewFinalFallComplete()
    {
        previewSequence = null;
        EnvironmentResourceViewUtility.SetChildrenVisible(view.transform, false);
        _stumpController.Show();
    }

    private void KillPreviewSequence()
    {
        if (previewSequence != null && previewSequence.IsActive())
        {
            previewSequence.Kill(false);
        }

        previewSequence = null;
    }

    private float GetFinalFallImpactDelay()
    {
        return finalFallSettings.MicroHoldDuration + finalFallSettings.MicroHoldDuration + finalFallSettings.FallDuration;
    }

    private Vector3 GetPreviewHitSource()
    {
        Vector3 previewSource = view.transform.position - view.transform.forward * 1.5f;
        previewSource.y = view.transform.position.y;
        return previewSource;
    }
}