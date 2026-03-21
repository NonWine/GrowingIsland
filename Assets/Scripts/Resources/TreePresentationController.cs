using System;
using Zenject;

public sealed class TreePresentationController : IInitializable, IDisposable , IResetable
{
    private readonly EnvironmentResourceEvents events;
    private readonly ITreeHitReaction hitReaction;
    private readonly TreeFinalFallSequence finalFallSequence;

    public TreePresentationController(
        EnvironmentResourceEvents events,
        ITreeHitReaction hitReaction,
        TreeFinalFallSequence finalFallSequence)
    {
        this.events = events;
        this.hitReaction = hitReaction;
        this.finalFallSequence = finalFallSequence;
    }

    public void Initialize()
    {
        events.HitApplied += OnHitApplied;
        events.RespawnCompleted += OnRespawnCompleted;
        Reset();
    }

    public void Dispose()
    {
        events.HitApplied -= OnHitApplied;
        events.RespawnCompleted -= OnRespawnCompleted;
    }

    public void Reset()
    {
        hitReaction.ResetToNeutral();
        finalFallSequence.Reset();
    }

    private void OnHitApplied(EnvironmentResourceHitEvent hitEvent)
    {
        if (hitEvent.IsFinalHit)
        {
            finalFallSequence.Play(hitEvent.SourceWorldPosition);
            return;
        }

        hitReaction.PlayHit(hitEvent.SourceWorldPosition);
    }

    private void OnRespawnCompleted()
    {
        Reset();
    }
}
