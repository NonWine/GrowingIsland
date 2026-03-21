public class AttackState : EnemyState
{
    private IAttackable attackable;
    private EnemyAnimator enemyAnimator;

    public AttackState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, IAttackable attackable) : base(enemyStateMachine, enemyAnimator)
    {
        this.enemyAnimator = enemyAnimator;
        this.attackable = attackable;
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        enemyAnimator.Attack();
    }

    public override void UpdateState()
    {
        if (!Enemy.IsPlayerInRange(Enemy.Stats.AttackRadius))
        {
            EnemyStateMachine.ChangeState<ChaseState>();
            return;
        }
        
        Enemy.EnemyRotator.RotateToPlayer();
        attackable.Attack();
    }

    public override void ExitState()
    {
        enemyAnimator.Idle();
    }
    
}

