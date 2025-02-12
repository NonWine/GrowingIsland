using Entity.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatroolState : EnemyState
{
    private Player _player;
    private OverlapSphereHandler _overlapSphereHandler;
    private EnemyAnimator _enemyAnimator;
    private PatroolArea _patroolArea;
    private NavMeshAgent _navMeshAgent;
    
    public EnemyPatroolState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, Player player, 
        PatroolArea patroolArea, NavMeshAgent agent) : base(enemyStateMachine, enemyAnimator)
    {
        _navMeshAgent = agent;
        _patroolArea = patroolArea;
        _enemyAnimator = enemyAnimator;
        _player = player;
        _overlapSphereHandler = new OverlapSphereHandler();
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz += _overlapSphereHandler.OnDrawGizmos;
        _navMeshAgent.SetDestination(_patroolArea.GetRandomPointInCollider());
        _enemyAnimator.Move();
    }

    public override void UpdateState()
    {
        if(Vector3.Distance(_player.transform.position, EnemyStateMachine.Enemy.transform.position) <= EnemyStateMachine.Enemy.Stats.RadiusDetection)
        {
            EnemyStateMachine.ChangeState<MoveState>();
        }

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
           EnemyStateMachine.ChangeState<EnemyIdleState>();
        }
        
    }

  
    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz -= _overlapSphereHandler.OnDrawGizmos;
    }
}