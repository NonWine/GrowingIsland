using System;
using UnityEngine;

public sealed class TreeFinalFallSequence : IDisposable
{
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;
    private readonly TreeResourceDropExecutor _resourceDropExecutor;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;
    private readonly TreeStumpController _stumpController;
    private readonly ITreeDelayScheduler delayScheduler;
    private readonly TreeFinalFallSettings treeHitAnimationSettings;
    private ITreeScheduledAction finalFallImpactAction;
    private ITreeScheduledAction finalFallCompleteAction;

    public TreeFinalFallSequence(
        EnvironmentPropObjectView view,
        EnvironmentResourceEvents events,
        TreeResourceDropExecutor resourceDropExecutor,
        ITreeHitReaction hitReaction,
        ITreeFinalFallReaction finalFallReaction,
        TreeStumpController stumpController,
        ITreeDelayScheduler delayScheduler,
        TreeFinalFallSettings treeHitAnimationSettings)
    {
        this.view = view;
        this.events = events;
        this._resourceDropExecutor = resourceDropExecutor;
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
        this._stumpController = stumpController;
        this.delayScheduler = delayScheduler;
        this.treeHitAnimationSettings = treeHitAnimationSettings;
    }

    public void Dispose()
    {
        CancelFinalFallImpact();
        CancelFinalFallComplete();
    }

    public void Play(Vector3 sourceWorldPosition)
    {
        CancelFinalFallImpact();
        CancelFinalFallComplete();

        finalFallReaction.Play(sourceWorldPosition);
        finalFallImpactAction = delayScheduler.Schedule(treeHitAnimationSettings.ImpactDelay, HandleFinalFallImpact);
    }

    public void Reset()
    {
        CancelFinalFallImpact();
        CancelFinalFallComplete();
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        _stumpController.Hide();
    }

    private void HandleFinalFallImpact()
    {
        finalFallImpactAction = null;
        _resourceDropExecutor.Spawn();
        finalFallCompleteAction = delayScheduler.Schedule(treeHitAnimationSettings.LandImpactDuration, HandleFinalFallComplete);
    }

    private void HandleFinalFallComplete()
    {
        finalFallCompleteAction = null;
        view.SetResourceVisualsVisible(false);
        _stumpController.Show();
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        events.RaisePresentationCompleted();
    }

    private void CancelFinalFallImpact()
    {
        finalFallImpactAction?.Cancel();
        finalFallImpactAction = null;
    }

    private void CancelFinalFallComplete()
    {
        finalFallCompleteAction?.Cancel();
        finalFallCompleteAction = null;
    }
}
