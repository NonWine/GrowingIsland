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
        _playerAnimator.SetStateBehaviour(1);
        
    }

    public override void LogicUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= _playerStats.MiningCD)
        {
            _timer = 0f;
            _damageableHandler.HandDamage(_playerStats.AxeDamage, out bool detected);
            if (detected == false)
            {
                stateMachine.ChangeState(PlayerStateKey.Idle);
            }
        }
    }

    public override void Exit()
    {
        _playerAnimator.SetStateBehaviour(0);
    }
}