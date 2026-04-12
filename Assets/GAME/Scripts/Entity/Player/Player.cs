using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IGameTickable
{
    [FormerlySerializedAs("_playerContainer")]
    [SerializeField] private PlayerContainer playerContainer;
    [FormerlySerializedAs("_healthUI")]
    [SerializeField] private HealthUI healthUI;
    private IGameController gameController;
    [ShowInInspector] private PlayerStateMachine playerStateMachine;

    public Transform ResourceStartPoint;

    private PlayerController playerController;
    private PlayerAnimator playerAnimator;
    private PlayerRotating playerRotating;
    private PlayerDefaultRadiusDamageHandler defaultRadiusDamageHandler;
    private PlayerAttackHandler playerAttackHandler;
    private TargetDetector targetDetector;
    private PlayerFarmTargetTracker farmTargetTracker;

    public PlayerContainer PlayerContainer => playerContainer;
    public PlayerStateMachine PlayerStateMachine => playerStateMachine;

    [Inject]
    private void Construct(PlayerController playerController,
        PlayerAnimator playerAnimator,
        PlayerStateMachine playerStateMachine,
        PlayerDefaultRadiusDamageHandler defaultRadiusDamageHandler,
        PlayerAttackHandler playerAttackHandler,
        TargetDetector targetDetector,
        PlayerFarmTargetTracker farmTargetTracker,
        IGameController gameController)
    {
        this.playerController = playerController;
        this.playerAnimator = playerAnimator;
        this.playerStateMachine = playerStateMachine;
        this.defaultRadiusDamageHandler = defaultRadiusDamageHandler;
        this.playerAttackHandler = playerAttackHandler;
        this.targetDetector = targetDetector;
        this.farmTargetTracker = farmTargetTracker;
        this.gameController = gameController;

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        gameController.RegisterInTick(this);
        InitializePlayerStateMachine();
        playerContainer.PlayerStats.CurrentHealth = playerContainer.PlayerStats.MaxHealth;
        healthUI.SetHealth(playerContainer.PlayerStats.MaxHealth);
    }

    private void InitializePlayerStateMachine()
    {
        var playerStates = new Dictionary<PlayerStateKey, PlayerState>
        {
            { PlayerStateKey.Idle, new PlayerIdleState(playerStateMachine, playerContainer) },
            { PlayerStateKey.Farming, new FarmingState(playerStateMachine, playerContainer, playerAnimator, defaultRadiusDamageHandler, farmTargetTracker) },
            {
                PlayerStateKey.Attack,new PlayerAttackState(playerStateMachine, playerContainer, playerAnimator, playerRotating, playerAttackHandler, targetDetector)
            }
        };

        playerStateMachine.RegisterStates(playerStates);
        playerStateMachine.Initialize(PlayerStateKey.Idle);
    }

    public void GetDamage(float dmg)
    {
        Debug.Log("Get Damage");
        playerContainer.PlayerStats.CurrentHealth -= dmg;
        healthUI.GetDamageUI(dmg);

        if (playerContainer.PlayerStats.CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Tick()
    {
        playerController.Tick();
    }

    private void OnDrawGizmos()
    {
        if (playerStateMachine != null)
        {
            playerStateMachine.CurrentState.OnDrawGizmos();
        }
    }
}

