using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyMoving : IEnemyMoveable
{
    protected PlayerContainer PlayerContainer;
    protected EnemyStateMachine EnemyStateMachine;
    protected NavMeshAgent NavMeshAgent;
    protected Vector3 SpawnPoint;
    
    public BaseEnemyMoving(NavMeshAgent navMeshAgent,PlayerContainer playerContainer, EnemyStateMachine enemyStateMachine, Vector3 spawnPoint)
    {
        PlayerContainer = playerContainer;
        EnemyStateMachine = enemyStateMachine;
        NavMeshAgent = navMeshAgent;
        SpawnPoint = spawnPoint;
    }

    public virtual void Move()
    {

        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerContainer.transform.position) <
            EnemyStateMachine.Enemy.Stats.TargetDistance)
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.velocity = Vector3.zero;
            EnemyStateMachine.ChangeState<AttackState>();
        }
        else
        {
            EnemyStateMachine.ChangeState<MoveState>();

        }
        
    }

    public virtual void StartMove()
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.SetDestination(PlayerContainer.transform.position);
    }
    
    
}