using System;
using DG.Tweening;
using UnityEngine;

public sealed class TreeFinalFallSequence : IDisposable
{
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;
    private readonly TreeResourceDropExecutor _resourceDropExecutor;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;
    private readonly TreeStumpController _stumpController;
    private readonly TreeFinalFallSettings treeHitAnimationSettings;
    private Sequence presentationSequence;

    public TreeFinalFallSequence(
        EnvironmentPropObjectView view,
        EnvironmentResourceEvents events,
        TreeResourceDropExecutor resourceDropExecutor,
        ITreeHitReaction hitReaction,
        ITreeFinalFallReaction finalFallReaction,
        TreeStumpController stumpController,
        TreeFinalFallSettings treeHitAnimationSettings)
    {
        this.view = view;
        this.events = events;
        this._resourceDropExecutor = resourceDropExecutor;
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
        this._stumpController = stumpController;
        this.treeHitAnimationSettings = treeHitAnimationSettings;
    }

    public void Dispose()
    {
        CancelPresentation();
    }

    public void Play(Vector3 sourceWorldPosition)
    {
        CancelPresentation();

        finalFallReaction.Play(sourceWorldPosition);
        presentationSequence = DOTween.Sequence()
            .AppendInterval(treeHitAnimationSettings.ImpactDelay)
            .AppendCallback(HandleFinalFallImpact)
            .AppendInterval(treeHitAnimationSettings.LandImpactDuration)
            .AppendCallback(HandleFinalFallComplete)
            .SetLink(view.gameObject);
    }

    public void Reset()
    {
        CancelPresentation();
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        _stumpController.Hide();
    }

    private void HandleFinalFallImpact()
    {
        _resourceDropExecutor.Spawn();
    }

    private void HandleFinalFallComplete()
    {
        presentationSequence = null;
        view.SetResourceVisualsVisible(false);
        _stumpController.Show();
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        events.RaisePresentationCompleted();
    }

    private void CancelPresentation()
    {
        if (presentationSequence != null && presentationSequence.IsActive())
        {
            presentationSequence.Kill(false);
        }

        presentationSequence = null;
    }
}
