using System;
using System.Linq;
using UnityEngine;

public class FarmingState : PlayerState
{
    private PlayerAnimator playerAnimator;
    private IDamageableHandler damageableHandler;
    private PlayerStats playerStats => player.PlayerStats;
    private float timer;
    
    public FarmingState( PlayerStateMachine stateMachine, PlayerContainer playerContainer
        , PlayerAnimator playerAnimator, 
        IDamageableHandler damageableHandler) : base( stateMachine, playerContainer)
    {
        this.playerAnimator = playerAnimator;
        this.damageableHandler = damageableHandler;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnFarming += TryLumber;
        playerAnimator.SetAnimataionLayerWeightBehaviour(1);
        playerAnimator.SetFarmingAnim(eCollectable.Wood);
        
    }

    public override void LogicUpdate()
    {
        
    }

    private void TryLumber()
    {
        damageableHandler.HandDamage(playerStats.AxeDamage, out bool detected, out Transform[] targets);
        
        if (detected == false)
        {
            stateMachine.ChangeState(PlayerStateKey.Idle);
        }
        else
        {
            
        }
    }

 
    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnFarming -= TryLumber;
        playerAnimator.SetAnimataionLayerWeightBehaviour(0);
        Debug.Log("Exit State");
    }
    
}

