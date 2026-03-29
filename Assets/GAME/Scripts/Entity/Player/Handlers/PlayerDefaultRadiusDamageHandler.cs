using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public class PlayerDefaultRadiusDamageHandler : IDamageableHandler
{
    private PlayerContainer playerContainer;
    private OverlapSphereHandler overlapSphereHandler;

    public PlayerDefaultRadiusDamageHandler(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler)
    {
        this.playerContainer = playerContainer;
        this.overlapSphereHandler = overlapSphereHandler;
    }



    private bool TryDamagingByRadius(float damage, out Transform[] taregt)
    {
        var enemies = Damageables();

        List<Transform> damagedTargets = new List<Transform>();
        foreach (var enemy in enemies)
        {
            ApplyDamage(enemy, damage);
            damagedTargets.Add(enemy.transform);
        }

        taregt = damagedTargets.ToArray();
        return taregt.Length > 0;    
    }

    private List<IDamageable> Damageables()
    {
        var enemies = overlapSphereHandler.GetFilteredObjects<IDamageable>(
            playerContainer.transform.position,
            playerContainer.PlayerStats.AggroRadius,
            0,
            enemy => enemy.isAlive
        );
        return enemies;
    }

    public void HandDamageSingleTarget(float damage, out IDamageable damagedTarget, Action Ifnull = null)
    {
        var enemies = Damageables();

        if (enemies.Count == 0)
        {
            Ifnull?.Invoke();
            damagedTarget = null;
            return;
        }
        
        List<Transform>  damagedTargets = new List<Transform>();
        enemies.ForEach(x => damagedTargets.Add(x.transform));
        
        damagedTarget = playerContainer.transform.GetNearestTarget(damagedTargets).GetComponent<IDamageable>();
        ApplyDamage(damagedTarget, damage);
    }
    

    public void HandDamage(float damage, out bool isDetected, out Transform[] taregt)
    {
        isDetected = TryDamagingByRadius(damage, out taregt);
    }

    private void ApplyDamage(IDamageable target, float damage)
    {
        if (target is IWorldHitDamageable worldHitDamageable)
        {
            worldHitDamageable.GetDamage(damage, playerContainer.transform.position);
            return;
        }

        target.GetDamage(damage);
    }
}

