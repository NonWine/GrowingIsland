using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerDamaging : IAttackable
{ 
    private PlayerContainer _playerContainer;
    private IDetector _detector;
    private float _timer;
    
    public PlayerDamaging(PlayerContainer playerContainer, IDetector detector)
    {
        _playerContainer = playerContainer;
        _detector = detector;
    }
    

    public void Attack()
    {

        foreach (var enemy in _detector.TryFindEnemies())
        {
            _timer += Time.deltaTime;
            if (_timer >= _playerContainer.PlayerStats.CoolDown)
            {
                _timer = 0f;
                enemy.GetDamage(_playerContainer.PlayerStats.Damage);
            }
        }
    }
}