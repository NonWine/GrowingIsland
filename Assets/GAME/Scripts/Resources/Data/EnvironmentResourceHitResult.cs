using UnityEngine;

public readonly struct EnvironmentResourceHitResult
{
    public EnvironmentResourceHitResult(float damage, float remainingHealth, bool isFinalHit, bool wasApplied, Vector3 sourceWorldPosition)
    {
        Damage = damage;
        RemainingHealth = remainingHealth;
        IsFinalHit = isFinalHit;
        SourceWorldPosition = sourceWorldPosition;
        WasApplied = wasApplied;
    }

    public float Damage { get; }
    public float RemainingHealth { get; }
    public bool IsFinalHit { get; }
    public bool WasApplied { get; }
    public Vector3 SourceWorldPosition { get; }
}