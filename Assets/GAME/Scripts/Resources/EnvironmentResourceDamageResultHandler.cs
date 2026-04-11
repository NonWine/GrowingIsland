using System;
using Zenject;

public interface IEnvironmentResourceDamageResultHandler : IInitializable, IDisposable
{
}

public sealed class StoneEnvironmentResourceDamageResultHandler : IEnvironmentResourceDamageResultHandler
{
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;

    public StoneEnvironmentResourceDamageResultHandler(EnvironmentPropObjectView view, EnvironmentResourceEvents events)
    {
        this.view = view;
        this.events = events;
    }

    public void Initialize()
    {
        events.FinalHitEvent += OnFinalHitApplied;
    }

    public void Dispose()
    {
        events.FinalHitEvent -= OnFinalHitApplied;
    }

    private void OnFinalHitApplied(EnvironmentResourceHitResult resourceHitResult)
    {
        EnvironmentResourceViewUtility.SetChildrenVisible(view.transform, false);
    }
}
