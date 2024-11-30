using System.Collections.Generic;
using System.Linq;
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

    private bool TryDamagingByRadius(float damage)
    {
        bool detectEnemy = false;
        // int count = Physics.OverlapSphereNonAlloc(
        //     _playerContainer.transform.position,
        //     _playerContainer.PlayerStats.RadiusDetection,
        //     _overlapResults
        // );
        //
        // for (int i = 0; i < count; i++)
        // {
        //     if (_overlapResults[i].TryGetComponent(out IDamageable damageable))
        //     {
        //         if (damageable.isAlive)
        //         {
        //             detectEnemy = true;
        //             damageable.GetDamage(damage);
        //         }
        //     }
        // }

        return detectEnemy;
    }

    private bool TryDamagingByRadius(float damage, out Transform[] taregt)
    {
        var enemies = _overlapSphereHandler.GetFilteredObjects<IDamageable>(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            0,
            enemy => enemy.isAlive
        );

        List<Transform> damagedTargets = new List<Transform>();
        foreach (var enemy in enemies)
        {
            enemy.GetDamage(damage);
            damagedTargets.Add(enemy.transform);
        }

        taregt = damagedTargets.ToArray();
        return taregt.Length > 0;    
    }
    

    public void HandDamage(float damage, out bool isDetected)
    {
        isDetected = TryDamagingByRadius(damage);
    }

    public void HandDamage(float damage, out bool isDetected, out Transform[] taregt)
    {
        isDetected = TryDamagingByRadius(damage, out taregt);
    }
}