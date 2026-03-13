using System;

public class PlayerAttackHandler
{
    private readonly float damage;

    public PlayerAttackHandler(float damage)
    {
        this.damage = damage;
    }

    public void TryAttack(IDamageable target, Action onMiss)
    {
        if (target == null || !target.isAlive)
        {
            onMiss?.Invoke();
            return;
        }

        target.GetDamage(damage);
    }
}

