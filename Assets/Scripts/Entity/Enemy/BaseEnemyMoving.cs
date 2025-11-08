using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyMoving : IEnemyMoveable
{
    protected PlayerContainer PlayerContainer;
    protected EnemyStateMachine EnemyStateMachine;
    protected NavMeshAgent NavMeshAgent;
    protected Vector3 SpawnPoint;

    protected BaseEnemy enemy => EnemyStateMachine.Enemy;
    
    public BaseEnemyMoving(NavMeshAgent navMeshAgent,PlayerContainer playerContainer, EnemyStateMachine enemyStateMachine, Vector3 spawnPoint)
    {
        PlayerContainer = playerContainer;
        EnemyStateMachine = enemyStateMachine;
        NavMeshAgent = navMeshAgent;
        SpawnPoint = spawnPoint;
    }

    public virtual void Move()
    {
        NavMeshAgent.SetDestination(PlayerContainer.transform.position);

        if (enemy.IsPlayerInRange(enemy.Stats.AttackRadius))
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.velocity = Vector3.zero;
            EnemyStateMachine.ChangeState<AttackState>();
        }
        
    }

    public virtual void StartMove()
    {
        NavMeshAgent.isStopped = false;
    }
    
    
}