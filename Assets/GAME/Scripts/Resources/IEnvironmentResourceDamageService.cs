using UnityEngine;

public interface IEnvironmentResourceDamageService
{
    bool IsAlive { get; }
    void ApplyDamage(float damage);
    void ApplyDamage(float damage, Vector3 sourceWorldPosition);
}

public interface IResetable
{
    void Reset();
}
