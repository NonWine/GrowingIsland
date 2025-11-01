using System;
using System.Linq;
using UnityEngine;

public class FarmingState : PlayerState
{
    private PlayerAnimator _playerAnimator;
    private IDamageableHandler _damageableHandler;
    private PlayerStats _playerStats => player.PlayerStats;
    private float _timer;
    
    public FarmingState( PlayerStateMachine stateMachine, PlayerContainer playerContainer
        , PlayerAnimator playerAnimator, 
        IDamageableHandler damageableHandler) : base( stateMachine, playerContainer)
    {
        _playerAnimator = playerAnimator;
        _damageableHandler = damageableHandler;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnFarming += TryLumber;
        _playerAnimator.SetAnimataionLayerWeightBehaviour(1);
        
    }

    public override void LogicUpdate()
    {
        
    }

    private void TryLumber()
    {
        _damageableHandler.HandDamage(_playerStats.AxeDamage, out bool detected, out Transform[] targets);
        
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
        _playerAnimator.SetAnimataionLayerWeightBehaviour(0);
    }
    
}