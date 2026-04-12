using System;
using UnityEngine;

public class PlayerDefaultRadiusDamageHandler : IDamageableHandler
{
    private readonly PlayerContainer playerContainer;

    public PlayerDefaultRadiusDamageHandler(PlayerContainer playerContainer)
    {
        this.playerContainer = playerContainer;
    }

    public bool TryDamageTarget(IDamageable target, float damage, Action ifNull = null)
    {
        if (target == null || !target.IsAlive)
        {
            ifNull?.Invoke();
            return false;
        }

        target.GetDamage(damage, playerContainer.transform.position);
        return true;
    }
}

