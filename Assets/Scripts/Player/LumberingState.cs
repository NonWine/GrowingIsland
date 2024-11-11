using UnityEngine;

public class LumberingState : PlayerState
{
    private PlayerAnimator _playerAnimator;
    private IDamageableHandler _damageableHandler;
    private PlayerStats _playerStats => player.PlayerStats;
    private float _timer;
    
    public LumberingState( PlayerStateMachine stateMachine, PlayerContainer playerContainer
        , PlayerAnimator playerAnimator, 
        IDamageableHandler damageableHandler) : base( stateMachine, playerContainer)
    {
        _playerAnimator = playerAnimator;
        _damageableHandler = damageableHandler;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnAxeAttacked += TryLumber;
        _playerAnimator.SetStateBehaviour(1);
        
    }

    public override void LogicUpdate()
    {
        
    }

    private void TryLumber()
    {
        _damageableHandler.HandDamage(_playerStats.AxeDamage, out bool detected, out Transform target);
        
        if (detected == false)
        {
            stateMachine.ChangeState(PlayerStateKey.Idle);
        }
        else
        {
            ParticlePool.Instance.PlayAxeHitFx(target.position);
        }
    }

    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnAxeAttacked -= TryLumber;
        _playerAnimator.SetStateBehaviour(0);
    }
}