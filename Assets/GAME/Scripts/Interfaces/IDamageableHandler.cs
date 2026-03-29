using System;
using UnityEngine;

public interface IDamageableHandler
{
    void HandDamage(float damageSend, out bool isDetected, out Transform[] targets);
    void HandDamageSingleTarget(float damage, out IDamageable damagedTarget, Action Ifnull = null);
}  