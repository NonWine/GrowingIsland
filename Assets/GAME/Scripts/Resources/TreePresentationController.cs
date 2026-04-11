using System;
using Zenject;

public sealed class TreePresentationController : IInitializable, IDisposable , IResetable
{
    private readonly EnvironmentResourceEvents events;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;

    public TreePresentationController(EnvironmentResourceEvents events, ITreeHitReaction hitReaction, ITreeFinalFallReaction finalFallReaction)
    {
        this.events = events;
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
    }

    public void Initialize()
    {
        events.HitApplied += OnHitApplied;
        events.FinalHitEvent += FinalHitApply;
        Reset();
    }

    public void Dispose()
    {
        events.HitApplied -= OnHitApplied;
        events.FinalHitEvent -= FinalHitApply;
    }

    public void Reset()
    {
        hitReaction.ResetToNeutral();
        finalFallReaction.ResetToNeutral();
    }

    private void OnHitApplied(EnvironmentResourceHitResult hitResult)
    {
        hitReaction.PlayHit(hitResult.SourceWorldPosition);
    }

    private void FinalHitApply(EnvironmentResourceHitResult hitResult)
    {
        finalFallReaction.Play(hitResult.SourceWorldPosition);
    }
}
