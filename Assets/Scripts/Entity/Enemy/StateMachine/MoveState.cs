using UnityEngine;
using UnityEngine.AI;

public class MoveState : EnemyState
{
    private IEnemyMoveable _moveable;
    private OverlapSphereHandler _overlapSphereHandler;
    
    public MoveState(EnemyStateMachine enemyStateMachine, EnemyAnimator enemyAnimator, IEnemyMoveable moveable) : base(enemyStateMachine, enemyAnimator)
    {
        _moveable = moveable;
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.EnemyHealth.OnGetDamage += AttackPlayer;
        EnemyAnimator.Move();
        _moveable.StartMove();
        
    }

    public override void UpdateState()
    {
        EnemyStateMachine.Enemy.EnemyRotator.RotateToPlayer();
        _moveable.Move();
    }

    public override void ExitState()
    {
        EnemyStateMachine.Enemy.EnemyHealth.OnGetDamage -= AttackPlayer;

    }
    
    private void AttackPlayer()
    {
        EnemyStateMachine.ChangeState<AttackState>();
    }

}