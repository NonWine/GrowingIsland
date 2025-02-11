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
        _navMeshAgent_.SetDestination(_spawnPoint);
        EnemyStateMachine.Enemy.OnGetDamage += AttackState;
        EnemyAnimator.Move();

    }

    public override void UpdateState()
    {
        if (!_navMeshAgent_.pathPending && _navMeshAgent_.remainingDistance <= _navMeshAgent_.stoppingDistance)
        {
            EnemyStateMachine.ChangeState<EnemyIdleState>();
        }

        if (Vector3.Distance(EnemyStateMachine.Enemy.transform.position, _player.transform.position) <
            EnemyStateMachine.Enemy.Stats.RadiusDetection  || EnemyStateMachine.Enemy.HasDamaged)
        {
            EnemyStateMachine.ChangeState<MoveState>();

        }
        
    }

    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnGetDamage -= AttackState;

    }

    private void AttackState() => EnemyStateMachine.ChangeState<AttackState>();
}