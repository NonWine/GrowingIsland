public class AttackState : EnemyState
{
    private IAttackable _attackable;
    private EnemyAnimator _enemyAnimator;

    public AttackState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, IAttackable attackable) : base(enemyStateMachine, enemyAnimator)
    {
        _enemyAnimator = enemyAnimator;
        _attackable = attackable;
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        _enemyAnimator.Attack();
    }

    public override void UpdateState()
    {
        if (!Enemy.IsPlayerInRange(Enemy.Stats.AttackRadius))
        {
            EnemyStateMachine.ChangeState<ChaseState>();
            return;
        }
        
        Enemy.EnemyRotator.RotateToPlayer();
        _attackable.Attack();
    }

    public override void ExitState()
    {
        _enemyAnimator.Idle();
    }
    
}