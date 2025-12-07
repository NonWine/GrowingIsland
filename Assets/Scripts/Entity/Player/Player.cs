using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IGameTickable
{
    [SerializeField] private PlayerContainer _playerContainer;
    [SerializeField] private HealthUI _healthUI;
    private IGameController _gameController;
    [ShowInInspector] private PlayerStateMachine _playerStateMachine;

    public Transform ResourceStartPoint;

    private PlayerController _playerController;
    private PlayerAnimator _playerAnimator;
    private PlayerRotating _playerRotating;
    private PlayerDefaultRadiusDamageHandler _defaultRadiusDamageHandler;
    private PlayerAttackHandler _playerAttackHandler;
    private TargetDetector _targetDetector;

    public PlayerContainer PlayerContainer => _playerContainer;
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;

    [Inject]
    private void Construct(PlayerController playerController,
        PlayerAnimator playerAnimator,
        PlayerRotating playerRotating,
        PlayerStateMachine playerStateMachine,
        PlayerDefaultRadiusDamageHandler defaultRadiusDamageHandler,
        PlayerAttackHandler playerAttackHandler,
        TargetDetector targetDetector,
        IGameController gameController)
    {
        _playerController = playerController;
        _playerAnimator = playerAnimator;
        _playerRotating = playerRotating;
        _playerStateMachine = playerStateMachine;
        _defaultRadiusDamageHandler = defaultRadiusDamageHandler;
        _playerAttackHandler = playerAttackHandler;
        _targetDetector = targetDetector;
        _gameController = gameController;

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        _gameController.RegisterInTick(this);
        InitializePlayerStateMachine();
        _playerContainer.PlayerStats.CurrentHealth = _playerContainer.PlayerStats.MaxHealth;
        _healthUI.SetHealth(_playerContainer.PlayerStats.MaxHealth);
    }

    private void InitializePlayerStateMachine()
    {
        var playerStates = new Dictionary<PlayerStateKey, PlayerState>
        {
            { PlayerStateKey.Idle, new PlayerIdleState(_playerStateMachine, _playerContainer) },
            { PlayerStateKey.Farming, new FarmingState(_playerStateMachine, _playerContainer, _playerAnimator, _defaultRadiusDamageHandler) },
            {
                PlayerStateKey.Attack,new PlayerAttackState(_playerStateMachine, _playerContainer, _playerAnimator, _playerRotating, _playerAttackHandler, _targetDetector)
            }
        };

        _playerStateMachine.RegisterStates(playerStates);
        _playerStateMachine.Initialize(PlayerStateKey.Idle);
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
        if (_playerStateMachine != null)
        {
            _playerStateMachine.CurrentState.OnDrawGizmos();
        }
    }
}
