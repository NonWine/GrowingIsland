using System;
using UnityEngine;

public readonly struct EnvironmentResourceHitEvent
{
    public EnvironmentResourceHitEvent(float damage, float remainingHealth, bool isFinalHit, Vector3 sourceWorldPosition)
    {
        Damage = damage;
        RemainingHealth = remainingHealth;
        IsFinalHit = isFinalHit;
        SourceWorldPosition = sourceWorldPosition;
    }

    public float Damage { get; }
    public float RemainingHealth { get; }
    public bool IsFinalHit { get; }
    public Vector3 SourceWorldPosition { get; }
}

public sealed class EnvironmentResourceEvents
{
    public event Action<EnvironmentResourceHitEvent> HitApplied;
    public event Action PresentationCompleted;
    public event Action RespawnCompleted;

    public void RaiseHitApplied(EnvironmentResourceHitEvent hitEvent)
    {
        HitApplied?.Invoke(hitEvent);
    }

    public void RaisePresentationCompleted()
    {
        PresentationCompleted?.Invoke();
    }

    public void RaiseRespawnCompleted()
    {
        RespawnCompleted?.Invoke();
    }
}
