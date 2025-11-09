using UnityEngine;
using UnityEngine.AI;

public class EnemyBackHomeState : EnemyState
{
    private Vector3 _spawnPoint;
    private NavMeshAgent _navMeshAgent_;
    private Player _player;
    
    public EnemyBackHomeState(EnemyAnimator enemyAnimator,  EnemyStateMachine enemyStateMachine, Player player, NavMeshAgent navMeshAgent, Vector3 spawnPoint) : base(enemyStateMachine, enemyAnimator)
    {
        _spawnPoint = spawnPoint;
        _player = player;
        _navMeshAgent_ = navMeshAgent;
    }

    public override void EnterState(BaseEnemy baseEnemy)
    {
        _navMeshAgent_.velocity = Vector3.zero;
        _navMeshAgent_.SetDestination(_spawnPoint);
        Enemy.EnemyHealth.OnGetDamage += AttackState;
        EnemyAnimator.Move();

    }

    public override void UpdateState()
    {
        Enemy.EnemyRotator.RotateToSpawnPoint();
        if (!_navMeshAgent_.pathPending && _navMeshAgent_.remainingDistance <= _navMeshAgent_.stoppingDistance)
        {
            EnemyStateMachine.ChangeState<EnemyIdleState>();
        }

        if (Enemy.IsPlayerInRange(Enemy.Stats.AggroRadius) || Enemy.EnemyHealth.HasDamaged)
        {
            EnemyStateMachine.ChangeState<ChaseState>();

        }
        
    }

    public override void ExitState()
    {
        Enemy.EnemyHealth.OnGetDamage -= AttackState;

    }

    private void AttackState() => EnemyStateMachine.ChangeState<AttackState>();
}