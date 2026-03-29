using UnityEngine;
using UnityEngine.AI;

public class EnemyMovingIdle : BaseEnemyMoving 
{
    
    public EnemyMovingIdle( Vector3 point, NavMeshAgent navMeshAgent, PlayerContainer playerContainer, EnemyStateMachine enemyStateMachine) 
        : base( navMeshAgent, playerContainer, enemyStateMachine, point)
    {
        
    }
    
    public override void Move()
    {
        if (Vector3.Distance(NavMeshAgent.transform.position, SpawnPoint) >
            enemy.Stats.LeashRadius && !enemy.EnemyHealth.HasDamaged && !enemy.IsPlayerInRange(enemy.Stats.AggroRadius))
        {
            EnemyStateMachine.ChangeState<EnemyBackHomeState>();
            return;
        }
            
        
        base.Move();
    }
}