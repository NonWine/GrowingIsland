using System;
using UnityEngine;

public class PlayerAttackHandler
{
    private readonly float damage;
    private readonly Transform attackSource;

    public PlayerAttackHandler(float damage, Transform attackSource)
    {
        this.damage = damage;
        this.attackSource = attackSource;
    }

    public void TryAttack(IDamageable target, Action onMiss)
    {
        if (target == null || !target.IsAlive)
        {
            onMiss?.Invoke();
            return;
        }

        target.GetDamage(damage, attackSource.position);
    }
}

