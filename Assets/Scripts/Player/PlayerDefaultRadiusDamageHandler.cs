using System.Linq;
using UnityEngine;

public class PlayerDefaultRadiusDamageHandler : IDamageableHandler
{
    private PlayerContainer _playerContainer;
    private Collider[] _overlapResults;

    public PlayerDefaultRadiusDamageHandler(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
        _overlapResults = new Collider[10];
    }

    private bool TryDamagingByRadius(float damage)
    {
        bool detectEnemy = false;
        int count = Physics.OverlapSphereNonAlloc(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            _overlapResults
        );

        for (int i = 0; i < count; i++)
        {
            if (_overlapResults[i].TryGetComponent(out IDamageable damageable))
            {
                if (damageable.isAlive)
                {
                    detectEnemy = true;
                    damageable.GetDamage(damage);
                }
            }
        }

        return detectEnemy;
    }

    public void HandDamage(float damage, out bool isDetected)
    {
        isDetected = TryDamagingByRadius(damage);
    }
}