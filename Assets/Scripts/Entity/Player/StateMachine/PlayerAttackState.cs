using Extensions;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private PlayerAnimator _playerAnimator;
    private PlayerRotating _playerRotating;
    private IDamageableHandler _damageableHandler;
    private PlayerStats _playerStats => player.PlayerStats;
    private Transform _currentTarget;
    private float _timer;
    
    public PlayerAttackState( PlayerStateMachine stateMachine, PlayerContainer playerContainer
        , PlayerAnimator playerAnimator, PlayerRotating playerRotating,
        IDamageableHandler damageableHandler) : base( stateMachine, playerContainer)
    {
        _playerAnimator = playerAnimator;
        _damageableHandler = damageableHandler;
        _playerRotating = playerRotating;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnFarming += TryLumber;
        _playerAnimator.SetAnimataionLayerWeightBehaviour(1);
        
    }

    public override void LogicUpdate()
    {
        Debug.Log(_currentTarget);
        if(_currentTarget == null)
            return;
        
        _playerRotating.SetTargetRotate(_currentTarget);
    }

    private void TryLumber()
    {
        _damageableHandler.HandDamage(_playerStats.Damage, out bool detected, out Transform[] targets);
        
        if (detected == false)
        {
            stateMachine.ChangeState(PlayerStateKey.Idle);
            return;
        }
        
        _currentTarget =  player.transform.GetNearestTarget(targets);

    }

 
    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnFarming -= TryLumber;
        _playerRotating.UnLockTarget();
        _playerAnimator.SetAnimataionLayerWeightBehaviour(0);
    }

    public void Dispose()
    {
        
    }
}

