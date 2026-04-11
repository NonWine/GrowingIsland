using UnityEngine;

public interface IEnvPropDamageService
{
    EnvironmentResourceHitResult ApplyDamage(float damage, Vector3 sourceWorldPosition);
}

public interface IResetable
{
    void Reset();
}
