using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerContainer _playerContainerPrefab;
    [SerializeField] private CameraFollowing _cameraFollowing;
    [SerializeField, ReadOnly] private List<IGameTickable> _gameTickable;
    private ObjectPoolTemplate<eCollectable,ResourcePartObj> _resourceObjectPool;
    private ObjectPoolTemplate<EnemyType, BaseEnemy> _poolTamplate;

    private DiContainer _diContainer;
    private PlayerContainer _playerContainer;
    private PlayerController _player;
    private PlayerStateMachine _playerStateMachine;
    private PlayerDefaultRadiusDamageHandler _defaultRadiusDamageHandler;
    
    
    [Inject]
    public void Construct(DiContainer diContainer)
    {
      _gameTickable = new List<IGameTickable>();
        _diContainer = diContainer;
    }

    public void RegisterInTick(IGameTickable tickable)
    {
        _gameTickable.Add(tickable);
    }

    private void Awake()
    {
        _playerContainer = _diContainer.InstantiatePrefabForComponent<PlayerContainer>(_playerContainerPrefab);
        _defaultRadiusDamageHandler = new PlayerDefaultRadiusDamageHandler(_playerContainer);
        RegisterPool();
        
        

        RegisterPlayerStateMachine();

        PlayerControllerInitialize();
        
        _playerContainer.PlayerTrigger.Init(_playerStateMachine);
        

        RegisterDependecies();




        _cameraFollowing.SetFollow(_playerContainer.transform);
    }

    private void RegisterPool()
    {
        _resourceObjectPool = new ObjectPoolTemplate<eCollectable, ResourcePartObj>(); 
        _diContainer.BindInstance(_resourceObjectPool).AsSingle();
    }

    private void PlayerControllerInitialize()
    {
        _player = new PlayerController
        (
            new PlayerMoving(_playerContainer),
            new PlayerRotating(_playerContainer),
            new PlayerAnimator(_playerContainer),
            new PlayerDamaging(_playerContainer, new PlayerEnemyDetector(_playerContainer, _poolTamplate)),
            _playerStateMachine
        );
    }

    private void RegisterPlayerStateMachine()
    {
        _playerStateMachine = new PlayerStateMachine();
        
        var playerStates = new Dictionary<PlayerStateKey, PlayerState>
        {
            { PlayerStateKey.Idle, new IdleState(_playerStateMachine, _playerContainer) },
            {
                PlayerStateKey.Lumber, new LumberingState(_playerStateMachine, _playerContainer, new PlayerAnimator(_playerContainer), _defaultRadiusDamageHandler)
            }
        };
        
        _playerStateMachine.RegisterStates(playerStates);
        _playerStateMachine.Initialize(PlayerStateKey.Idle);

    }

    private void RegisterDependecies()
    {
        _diContainer.BindInstance(_playerContainer).AsSingle().Lazy();
        _diContainer.BindInstance(_playerStateMachine).AsSingle();
        
    }


    private void Update()
    {
        foreach (var gameTickable in _gameTickable)
        {
            gameTickable.Tick();
        }

        _player.Tick();
    }
}