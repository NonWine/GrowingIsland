using System;
public interface IDamageableHandler
{
    bool TryDamageTarget(IDamageable target, float damage, Action ifNull = null);
}  
