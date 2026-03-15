using UnityEngine;
using Zenject;

public class PropsDamageService : IEnvironmentResourceDamageService, IResetable, IInitializable
{
    private readonly ResourceWorld resourceWorld;
    private readonly EnvironmentPropObjectView view;
    private readonly EnvironmentResourceEvents events;

    private float health;

    public PropsDamageService(
        ResourceWorld resourceWorld,
        EnvironmentPropObjectView view,
        EnvironmentResourceEvents events)
    {
        this.resourceWorld = resourceWorld;
        this.view = view;
        this.events = events;
    }

    public bool IsAlive { get; private set; }

    public void Initialize()
    {
        Reset();
        view.SetResourceVisualsVisible(true);
    }

    public void Reset()
    {
        IsAlive = true;
        health = resourceWorld.Health;
    }

    public void ApplyDamage(float damage)
    {
        ApplyDamage(damage, view.GetDefaultHitSource());
    }

    public void ApplyDamage(float damage, Vector3 sourceWorldPosition)
    {
        if (!IsAlive)
        {
            return;
        }

        health -= damage;
        bool isFinalHit = health <= 0f;
        if (isFinalHit)
        {
            IsAlive = false;
        }

        events.RaiseHitApplied(new EnvironmentResourceHitEvent(damage, health, isFinalHit, sourceWorldPosition));
    }
}
