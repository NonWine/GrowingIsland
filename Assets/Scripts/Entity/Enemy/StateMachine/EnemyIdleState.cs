public class EnemyIdleState : EnemyState
{
    private OverlapSphereHandler _overlapSphereHandler;
    private EnemyAnimator _enemyAnimator;
    
    public EnemyIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine, enemyAnimator)
    {
        _enemyAnimator = enemyAnimator;
        _overlapSphereHandler = new OverlapSphereHandler();
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz += _overlapSphereHandler.OnDrawGizmos;
        _enemyAnimator.Idle();
        
    }

    public override void UpdateState()
    {
        if(EnemyStateMachine.Enemy.IsPlayerInRange(EnemyStateMachine.Enemy.Stats.AggroRadius))
        {
            EnemyStateMachine.ChangeState<ChaseState>();
        }
    }

  
    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz -= _overlapSphereHandler.OnDrawGizmos;
    }
}