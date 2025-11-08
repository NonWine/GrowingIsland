using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private IEnemyMoveable _moveable;
    private OverlapSphereHandler _overlapSphereHandler;
    
    public ChaseState(EnemyStateMachine enemyStateMachine, EnemyAnimator enemyAnimator, IEnemyMoveable moveable) : base(enemyStateMachine, enemyAnimator)
    {
        _moveable = moveable;
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        Enemy.EnemyHealth.OnGetDamage += AttackPlayer;
        EnemyAnimator.Move();
        _moveable.StartMove();
        
    }

    public override void UpdateState()
    {
        Enemy.EnemyRotator.RotateToPlayer();
        _moveable.Move();
    }

    public override void ExitState()
    {
        Enemy.EnemyHealth.OnGetDamage -= AttackPlayer;

    }
    
    private void AttackPlayer()
    {
        EnemyStateMachine.ChangeState<AttackState>();
    }

}