public class EnemyIdleState : EnemyState
{
    private OverlapSphereHandler overlapSphereHandler;
    private EnemyAnimator enemyAnimator;
    
    public EnemyIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine, enemyAnimator)
    {
        this.enemyAnimator = enemyAnimator;
        overlapSphereHandler = new OverlapSphereHandler();
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz += overlapSphereHandler.OnDrawGizmos;
        enemyAnimator.Idle();
        
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
        EnemyStateMachine.Enemy.OnDrawGizmoz -= overlapSphereHandler.OnDrawGizmos;
    }
}

