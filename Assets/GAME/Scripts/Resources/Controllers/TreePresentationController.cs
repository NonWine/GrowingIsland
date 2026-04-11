using System;
using Cysharp.Threading.Tasks;
using Zenject;

public sealed class TreePresentationController : IInitializable, IDisposable, IResetable, IFinalHitPresentation
{
    private readonly EnvironmentResourceEvents events;
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;

    public TreePresentationController(
        EnvironmentResourceEvents events,
        ITreeHitReaction hitReaction,
        ITreeFinalFallReaction finalFallReaction)
    {
        this.events = events;
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
    }

    public void Initialize()
    {
        events.HitApplied += OnHitApplied;
        Reset();
    }

    public void Dispose()
    {
        events.HitApplied -= OnHitApplied;
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

    public async UniTask PlayAsync(EnvironmentResourceHitResult hitResult)
    {
        await finalFallReaction.Play(hitResult.SourceWorldPosition);
    }
}
