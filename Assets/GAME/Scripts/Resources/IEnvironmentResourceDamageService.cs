using UnityEngine;

public interface IEnvironmentResourceDamageService
{
    EnvironmentResourceHitResult ApplyDamage(float damage, Vector3 sourceWorldPosition);
}

public interface IResetable
{
    void Reset();
}
