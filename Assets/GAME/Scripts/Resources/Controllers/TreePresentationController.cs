using System;
using Cysharp.Threading.Tasks;
using Zenject;

public sealed class TreePresentationController : IInitializable, IResetable, IHitPresentation
{
    private readonly ITreeHitReaction hitReaction;
    private readonly ITreeFinalFallReaction finalFallReaction;

    public TreePresentationController(ITreeHitReaction hitReaction, ITreeFinalFallReaction finalFallReaction)
    {
        this.hitReaction = hitReaction;
        this.finalFallReaction = finalFallReaction;
    }

    public void Initialize()
    {
        Reset();
    }

    public void Reset()
    {
        hitReaction.ResetToNeutral();
        finalFallReaction.ResetToNeutral();
    }

    public async UniTask PlayHitAsync(EnvironmentResourceHitResult hitResult)
    {
        UniTask hitTask = hitReaction.Play(hitResult.SourceWorldPosition);

        if (hitResult.IsFinalHit)
        {
            await hitTask;
            return;
        }

        await hitTask;
    }

    public async UniTask PlayFinalHitAsync(EnvironmentResourceHitResult hitResult)
    {
        await finalFallReaction.Play(hitResult.SourceWorldPosition);
    }
}
