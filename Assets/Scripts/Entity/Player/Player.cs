using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IGameTickable
{
    [SerializeField] private PlayerContainer _playerContainer;
    [SerializeField] private HealthUI _healthUI;
    [Inject] private IGameСontroller _gameСontroller;
    [Inject] private OverlapSphereHandler _overlapSphereHandler;
    [ShowInInspector] private PlayerStateMachine _playerStateMachine;
    private PlayerHandlersService _playerHandlersService;
    private PlayerController _playerController;
    private PlayerAnimator _playerAnimator;
    private PlayerFarmDetector _playerFarmDetector;
    private PlayerEnemyDetector _playerEnemyDetector;
    private PlayerRotating _playerRotating;
    public Transform ResourceStartPoint;
    
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    
    public PlayerController PlayerController => _playerController;

    public PlayerContainer PlayerContainer => _playerContainer;
    
    private void Awake()
    {
        _gameСontroller.RegisterInTick(this);
        InitializeHandler();
        InitializePlayerStateMachine();
        _playerFarmDetector = new PlayerFarmDetector(_playerContainer, _overlapSphereHandler, _playerStateMachine, _playerAnimator);
        _playerEnemyDetector = new PlayerEnemyDetector(_playerContainer, _overlapSphereHandler, _playerStateMachine, _playerAnimator);
        
        PlayerInitialize();
        _playerContainer.PlayerStats.CurrentHealth = _playerContainer.PlayerStats.MaxHealth;
        _healthUI.SetHealth(_playerContainer.PlayerStats.MaxHealth);
    }
    
    private void PlayerInitialize()
    {
        _playerController = new PlayerController
        (
            new PlayerMoving(_playerContainer),
            _playerRotating,
            _playerAnimator,
            _playerHandlersService.PlayerResourceDetector,
            _playerStateMachine,
            _playerFarmDetector,
            _playerEnemyDetector
        );
        
    }
    
    private void InitializePlayerStateMachine()
    {
        _playerStateMachine = new PlayerStateMachine();
        
        var playerStates = new Dictionary<PlayerStateKey, PlayerState>
        {
            { PlayerStateKey.Idle, new PlayerIdleState(_playerStateMachine, _playerContainer) },
            {
                PlayerStateKey.Farming, new FarmingState(_playerStateMachine, _playerContainer,_playerAnimator, _playerHandlersService.DefaultRadiusDamageHandler)
            },
            
             {
                PlayerStateKey.Attack, new PlayerAttackState(_playerStateMachine, _playerContainer,
                    _playerAnimator,_playerRotating, _playerHandlersService.playerAttackHandler, _playerHandlersService.targetDetector )
            }
        };
        
        _playerStateMachine.RegisterStates(playerStates);
        _playerStateMachine.Initialize(PlayerStateKey.Idle);

    }

    private void InitializeHandler()
    {
        _playerHandlersService = new PlayerHandlersService(_playerContainer, _overlapSphereHandler);
        _playerAnimator = new PlayerAnimator(_playerContainer);
        _playerRotating = new PlayerRotating(_playerContainer);
    }

    public void GetDamage(float dmg)
    {
        Debug.Log("Get Damage");
        _playerContainer.PlayerStats.CurrentHealth -= dmg;
        _healthUI.GetDamageUI(dmg);

        if (_playerContainer.PlayerStats.CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Tick()
    {
        _playerController.Tick();
    }

    private void OnDrawGizmos()
    {   
        if(_playerStateMachine != null)
            _playerStateMachine.CurrentState.OnDrawGizmos();
    }
}