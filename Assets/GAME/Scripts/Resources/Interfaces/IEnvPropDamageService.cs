using UnityEngine;

public interface IEnvPropDamageService
{
    EnvironmentResourceHitResult ApplyDamage(float damage, Vector3 sourceWorldPosition);
}