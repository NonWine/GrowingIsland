using System;
using Zenject;

public interface IEnvironmentResourceDamageResultHandler : IInitializable, IDisposable
{
}

public sealed class DefaultEnvironmentResourceDamageResultHandler : IEnvironmentResourceDamageResultHandler
{
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;

    public DefaultEnvironmentResourceDamageResultHandler(EnvironmentPropObjectView view, EnvironmentResourceEvents events)
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
    }

    private void OnHitApplied(EnvironmentResourceHitEvent hitEvent)
    {
        if (!hitEvent.IsFinalHit)
        {
            return;
        }

        view.SetResourceVisualsVisible(false);
        events.RaisePresentationCompleted();
    }
}

public sealed class StoneEnvironmentResourceDamageResultHandler : IEnvironmentResourceDamageResultHandler
{
    private readonly ResourceWorld resourceWorld;
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceDropSpawner dropSpawner;
    private readonly EnvironmentResourceEvents events;

    public StoneEnvironmentResourceDamageResultHandler(
        ResourceWorld resourceWorld,
        EnvironmentPropObjectView view,
        EnvironmentResourceDropSpawner dropSpawner,
        EnvironmentResourceEvents events)
    {
        this.resourceWorld = resourceWorld;
        this.view = view;
        this.dropSpawner = dropSpawner;
        this.events = events;
    }

    public void Initialize()
    {
        events.HitApplied += OnHitApplied;
    }

    public void Dispose()
    {
        events.HitApplied -= OnHitApplied;
    }

    private void OnHitApplied(EnvironmentResourceHitEvent hitEvent)
    {
        ParticlePool.Instance.PlayMineHitFx(view.transform.position);
        if (!hitEvent.IsFinalHit)
        {
            return;
        }

        dropSpawner.Spawn(resourceWorld.TypeWallet, resourceWorld.VisualDrop, view.transform.position);
        view.SetResourceVisualsVisible(false);
        events.RaisePresentationCompleted();
    }
}
