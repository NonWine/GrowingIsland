using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnvironmentPropObjectView))]
public class TreeDebugPreview : MonoBehaviour
{
    [Inject(Optional = true)] private TreePreviewController preview;

    [ContextMenu("Debug/Preview Hit")]
    private void PreviewHit()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewHit();
    }

    [ContextMenu("Debug/Preview Final Fall")]
    private void PreviewFinalFall()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewFinalFall();
    }

    [ContextMenu("Debug/Preview Full Final Hit")]
    private void PreviewFullFinalHit()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.PreviewFullFinalHit();
    }

    [ContextMenu("Debug/Reset Preview")]
    private void ResetPreview()
    {
        if (!Application.isPlaying || preview == null)
        {
            return;
        }

        preview.ResetPreview();
    }
}

public sealed class TreePreviewController : IDisposable
{
    private readonly EnvironmentPropObjectView view;
    private readonly TreeFinalFallSettings finalFallSettings;
    private readonly TreeResourceDropExecutor _resourceDropExecutor;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;
    private readonly TreeStumpController _stumpController;

    private Tween finalFallImpactTween;
    private Tween finalFallCompleteTween;

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
        KillFinalFallImpactTween();
        KillFinalFallCompleteTween();
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
        finalFallReaction.Play(GetPreviewHitSource());
        ScheduleFinalFallImpact(HandlePreviewFinalFallImpact);
    }

    public void ResetPreview()
    {
        KillFinalFallImpactTween();
        KillFinalFallCompleteTween();
        hitReaction.ResetToNeutral();
        finalFallReaction.ResetToNeutral();
        _stumpController.Hide();
        view.SetResourceVisualsVisible(true);
    }

    private void HandlePreviewFinalFallImpact()
    {
        _resourceDropExecutor.Spawn();
        ScheduleFinalFallComplete(HandlePreviewFinalFallComplete);
    }

    private void HandlePreviewFinalFallComplete()
    {
        view.SetResourceVisualsVisible(false);
        _stumpController.Show();
    }

    private void ScheduleFinalFallImpact(TweenCallback onImpact)
    {
        KillFinalFallImpactTween();
        finalFallImpactTween = DOVirtual.DelayedCall(GetFinalFallImpactDelay(), onImpact)
            .SetLink(view.gameObject);
    }

    private void ScheduleFinalFallComplete(TweenCallback onComplete)
    {
        KillFinalFallCompleteTween();
        finalFallCompleteTween = DOVirtual.DelayedCall(finalFallSettings.LandImpactDuration, onComplete)
            .SetLink(view.gameObject);
    }

    private void KillFinalFallImpactTween()
    {
        if (finalFallImpactTween != null && finalFallImpactTween.IsActive())
        {
            finalFallImpactTween.Kill(false);
        }
    }

    private void KillFinalFallCompleteTween()
    {
        if (finalFallCompleteTween != null && finalFallCompleteTween.IsActive())
        {
            finalFallCompleteTween.Kill(false);
        }
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
