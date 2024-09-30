using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyDetector : IDetector
{
    private ObjectPoolEnemy _objectPoolEnemy;
    private PlayerContainer _playerContainer;
    private List<BaseEnemy> _enemies;
    private List<BaseEnemy> _enemiesToRemove;
 
    
    public PlayerEnemyDetector(PlayerContainer playerContainer,
        ObjectPoolEnemy objectPoolEnemy)
    {
        _playerContainer = playerContainer;
        _objectPoolEnemy = objectPoolEnemy;
        _enemies = new List<BaseEnemy>();
        _enemiesToRemove = new List<BaseEnemy>();

    }
    
    
    public List<BaseEnemy> TryFindEnemies()
    {
        foreach (var enemy in _objectPoolEnemy._InActiveUnits)
        {
            var distance = Vector3.Distance(_playerContainer.transform.position, enemy.transform.position);
            var stats = _playerContainer.PlayerStats;
            
            if (!enemy.Stats.IsDeath && !_enemies.Contains(enemy) &&
                distance <= stats.RadiusDetection  && _enemies.Count < stats.MaxEnemyDetection)
            {
                _enemies.Add(enemy);
            }
            else if (_enemies.Contains(enemy) &&
                     (distance >= stats.RadiusDetection || enemy.Stats.IsDeath))
            {
                _enemiesToRemove.Add(enemy);
            }
        }

        TryToClearEnemies();

        return _enemies;
    }

    private void TryToClearEnemies()
    {
        foreach (var enemy in _enemiesToRemove)
        {
            _enemies.Remove(enemy);
        }

        _enemiesToRemove.Clear();
    }
}