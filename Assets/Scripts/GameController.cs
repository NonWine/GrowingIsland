using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerContainer _playerContainerPrefab;
    [SerializeField] private CameraFollowing _cameraFollowing;
    private ObjectPoolEnemy _objectPoolEnemy;
    private DiContainer _diContainer;
    private PlayerContainer _playerContainer;
    private PlayerController _player;

    [Inject]
    public void Construct(ObjectPoolEnemy objectPoolEnemy, DiContainer diContainer)
    {
        _objectPoolEnemy = objectPoolEnemy;
        _diContainer = diContainer;
    }
    
    private void Awake()
    {
        _playerContainer = _diContainer.InstantiatePrefabForComponent<PlayerContainer>(_playerContainerPrefab);
        _diContainer.BindInstance(_playerContainer).AsSingle().Lazy();
        
        _player = new PlayerController
        (
            new PlayerMoving(_playerContainer),
            new PlayerRotating(_playerContainer),
            new PlayerAnimator(_playerContainer),
            new PlayerDamaging(_playerContainer, new PlayerEnemyDetector(_playerContainer,_objectPoolEnemy)));

        _cameraFollowing.Init(_playerContainer.transform);
    }

    private void Update()
    {
        foreach (var enemy in _objectPoolEnemy._InActiveUnits)
        {
            enemy.Tick();
        }
        _player.Tick();
    }
}
