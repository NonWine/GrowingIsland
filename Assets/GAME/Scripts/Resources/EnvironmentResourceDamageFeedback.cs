using System;
using DG.Tweening;
using Zenject;

public interface IEnvironmentResourceDamageFeedback : IInitializable, IDisposable
{
}

public sealed class ScalePunchEnvironmentResourceDamageFeedback : IEnvironmentResourceDamageFeedback
{
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;

    public ScalePunchEnvironmentResourceDamageFeedback(EnvironmentPropObjectView view, EnvironmentResourceEvents events)
    {
        this.view = view;
        this.events = events;
    }

    public void Initialize()
    {
        events.HitApplied += OnHitApplied;
    }

    public void Dispose()
    {
        events.HitApplied -= OnHitApplied;
        view.transform.DOKill();
    }

    private void OnHitApplied(EnvironmentResourceHitEvent hitEvent)
    {
        view.transform.DOKill();
        view.transform.DOScale(1.05f, 0.15f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                view.transform.DOScale(1f, 0.15f)
                    .SetEase(Ease.OutBounce);
            });
    }
}
