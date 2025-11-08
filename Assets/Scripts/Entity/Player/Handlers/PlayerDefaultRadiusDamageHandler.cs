using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public class PlayerDefaultRadiusDamageHandler : IDamageableHandler
{
    private PlayerContainer _playerContainer;
    private OverlapSphereHandler _overlapSphereHandler;

    public PlayerDefaultRadiusDamageHandler(PlayerContainer playerContainer, OverlapSphereHandler overlapSphereHandler)
    {
        _playerContainer = playerContainer;
        _overlapSphereHandler = overlapSphereHandler;
    }



    private bool TryDamagingByRadius(float damage, out Transform[] taregt)
    {
        var enemies = Damageables();

        List<Transform> damagedTargets = new List<Transform>();
        foreach (var enemy in enemies)
        {
            enemy.GetDamage(damage);
            damagedTargets.Add(enemy.transform);
        }

        taregt = damagedTargets.ToArray();
        return taregt.Length > 0;    
    }

    private List<IDamageable> Damageables()
    {
        var enemies = _overlapSphereHandler.GetFilteredObjects<IDamageable>(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.AggroRadius,
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
        
        damagedTarget = _playerContainer.transform.GetNearestTarget(damagedTargets).GetComponent<IDamageable>();
        damagedTarget.GetDamage(damage);
    }
    

    public void HandDamage(float damage, out bool isDetected, out Transform[] taregt)
    {
        isDetected = TryDamagingByRadius(damage, out taregt);
    }
}