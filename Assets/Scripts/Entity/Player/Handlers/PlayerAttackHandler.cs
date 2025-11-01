using System;

public class PlayerAttackHandler
{
    private readonly float _damage;

    public PlayerAttackHandler(float damage)
    {
        _damage = damage;
    }

    public void TryAttack(IDamageable target, Action onMiss)
    {
        if (target == null || !target.isAlive)
        {
            onMiss?.Invoke();
            return;
        }

        target.GetDamage(_damage);
    }
}