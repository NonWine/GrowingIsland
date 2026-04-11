using System;
using Zenject;

public sealed class TreePresentationController : IInitializable, IDisposable , IResetable
{
    private readonly EnvironmentResourceEvents events;
    private readonly ITreeHitReaction hitReaction;
    private readonly TreeFinalFallSequence finalFallSequence;

    public TreePresentationController(EnvironmentResourceEvents events, ITreeHitReaction hitReaction, TreeFinalFallSequence finalFallSequence)
    {
        this.events = events;
        this.hitReaction = hitReaction;
        this.finalFallSequence = finalFallSequence;
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
        finalFallSequence.Reset();
    }

    private void OnHitApplied(EnvironmentResourceHitResult hitResult)
    {
        hitReaction.PlayHit(hitResult.SourceWorldPosition);
    }

    private void FinalHitApply(EnvironmentResourceHitResult hitResult)
    {
        finalFallSequence.Play(hitResult.SourceWorldPosition);
    }
    
}
