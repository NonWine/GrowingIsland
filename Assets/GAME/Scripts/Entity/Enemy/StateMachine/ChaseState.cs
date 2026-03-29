using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private IEnemyMoveable moveable;
    private OverlapSphereHandler overlapSphereHandler;
    
    public ChaseState(EnemyStateMachine enemyStateMachine, EnemyAnimator enemyAnimator, IEnemyMoveable moveable) : base(enemyStateMachine, enemyAnimator)
    {
        this.moveable = moveable;
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        Enemy.EnemyHealth.OnGetDamage += AttackPlayer;
        EnemyAnimator.Move();
        moveable.StartMove();
        
    }

    public override void UpdateState()
    {
        Enemy.EnemyRotator.RotateToPlayer();
        moveable.Move();
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

