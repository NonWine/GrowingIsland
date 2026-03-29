using UnityEngine;

public interface IWorldHitDamageable : IDamageable
{
    void GetDamage(float damage, Vector3 sourceWorldPosition);
}
