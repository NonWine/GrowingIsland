using System;
using UnityEngine;

public sealed class EnvironmentResourceEvents
{
    public event Action<EnvironmentResourceHitResult> HitApplied;
    public event Action<float, Vector3> OnReceiveWorldDamage;
    public event Action<EnvironmentResourceHitResult> FinalHitEvent;

    public void RaiseReceiveWorldDamage(float damage, Vector3 position)
    {
        OnReceiveWorldDamage?.Invoke(damage, position);
    }
    
    public void RaiseHitApplied(EnvironmentResourceHitResult hitResult)
    {
        HitApplied?.Invoke(hitResult);
    }

    public void RaiseFinalHitEvent(EnvironmentResourceHitResult hitResult)
    {
        FinalHitEvent?.Invoke(hitResult);
    }
}
