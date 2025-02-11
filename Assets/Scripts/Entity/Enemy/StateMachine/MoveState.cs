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
        EnemyStateMachine.Enemy.OnGetDamage += AttackPlayer;
        EnemyAnimator.Move();
        _moveable.StartMove();
        
    }

    public override void UpdateState()
    {
        EnemyStateMachine.Enemy.SetTargetPlayer();
        _moveable.Move();
    }

    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnGetDamage -= AttackPlayer;

    }
    
    private void AttackPlayer()
    {
        EnemyStateMachine.ChangeState<AttackState>();
    }

}