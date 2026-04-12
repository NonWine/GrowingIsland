using UnityEngine;

public class FarmingState : PlayerState
{
    private readonly PlayerAnimator playerAnimator;
    private readonly IDamageableHandler damageableHandler;
    private readonly PlayerFarmTargetTracker farmTargetTracker;
    private PlayerStats playerStats => player.PlayerStats;
    
    public FarmingState( PlayerStateMachine stateMachine, PlayerContainer playerContainer
        , PlayerAnimator playerAnimator, 
        IDamageableHandler damageableHandler,
        PlayerFarmTargetTracker farmTargetTracker) : base( stateMachine, playerContainer)
    {
        this.playerAnimator = playerAnimator;
        this.damageableHandler = damageableHandler;
        this.farmTargetTracker = farmTargetTracker;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnFarming += TryLumber;
        playerAnimator.SetAnimataionLayerWeightBehaviour(1);
        playerAnimator.SetFarmingAnim(eCollectable.Wood);
        
    }

    public override void LogicUpdate()
    {
        if (!farmTargetTracker.HasTarget)
        {
            stateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }

    private void TryLumber()
    {
        if (!damageableHandler.TryDamageTarget(farmTargetTracker.CurrentTarget, playerStats.AxeDamage))
        {
            stateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }

 
    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnFarming -= TryLumber;
        playerAnimator.SetAnimataionLayerWeightBehaviour(0);
        farmTargetTracker.Clear();
    }
    
}

