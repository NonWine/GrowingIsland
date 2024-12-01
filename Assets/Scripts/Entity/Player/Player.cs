using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IGameTickable
{
    [SerializeField] private PlayerContainer _playerContainer;
    [Inject] private IGameСontroller _gameСontroller;
    [Inject] private OverlapSphereHandler _overlapSphereHandler;
    private PlayerHandlersService _playerHandlersService;
    private PlayerController _playerController;
    private PlayerStateMachine _playerStateMachine;
    private PlayerAnimator _playerAnimator;
    private PlayerFarmDetector _playerFarmDetector;
    public Transform ResourceStartPoint;
    
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    

    public PlayerController PlayerController => _playerController;

    public PlayerContainer PlayerContainer => _playerContainer;
    
    private void Awake()
    {
        _gameСontroller.RegisterInTick(this);
        InitializeHandler();
        _playerAnimator = new PlayerAnimator(_playerContainer);
        InitializePlayerStateMachine();
        _playerFarmDetector = new PlayerFarmDetector(_playerContainer, _overlapSphereHandler, _playerStateMachine, _playerAnimator);
        PlayerInitialize();
        
    }
    
    private void PlayerInitialize()
    {
        
        _playerController = new PlayerController
        (
            new PlayerMoving(_playerContainer),
            new PlayerRotating(_playerContainer),
            _playerAnimator,
            _playerHandlersService.PlayerResourceDetector,
            _playerStateMachine,
            _playerFarmDetector
        );
    }
    
    private void InitializePlayerStateMachine()
    {
        _playerStateMachine = new PlayerStateMachine();
        
        var playerStates = new Dictionary<PlayerStateKey, PlayerState>
        {
            { PlayerStateKey.Idle, new IdleState(_playerStateMachine, _playerContainer) },
            {
                PlayerStateKey.Farming, new FarmingState(_playerStateMachine, _playerContainer,_playerAnimator, _playerHandlersService.DefaultRadiusDamageHandler)
            }
        };
        
        _playerStateMachine.RegisterStates(playerStates);
        _playerStateMachine.Initialize(PlayerStateKey.Idle);

    }

    private void InitializeHandler()
    {
        _playerHandlersService = new PlayerHandlersService(_playerContainer, _overlapSphereHandler);
    }

    public void Tick()
    {
      //  _overlapSphereHandler.UpdateOverlapSphere();
        _playerController.Tick();
    }

    private void OnDrawGizmos()
    {
        if(_overlapSphereHandler != null)
            _overlapSphereHandler.OnDrawGizmos();
    }
}