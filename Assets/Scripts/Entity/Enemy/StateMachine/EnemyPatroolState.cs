using UnityEngine;
using UnityEngine.AI;

public class EnemyPatroolState : EnemyState
{
    private Player player;
    private OverlapSphereHandler overlapSphereHandler;
    private EnemyAnimator enemyAnimator;
    private PatrolArea patroolArea;
    private NavMeshAgent navMeshAgent;
    
    public EnemyPatroolState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, Player player, 
        PatrolArea patroolArea, NavMeshAgent agent) : base(enemyStateMachine, enemyAnimator)
    {
        navMeshAgent = agent;
        this.patroolArea = patroolArea;
        this.enemyAnimator = enemyAnimator;
        this.player = player;
        overlapSphereHandler = new OverlapSphereHandler();
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz += overlapSphereHandler.OnDrawGizmos;
        navMeshAgent.SetDestination(patroolArea.GetRandomPointInCollider());
        enemyAnimator.Move();
    }

    public override void UpdateState()
    {
        Enemy.EnemyRotator.RotateToPoint(navMeshAgent.destination);
        if(Enemy.IsPlayerInRange(Enemy.Stats.AggroRadius))
        {
            EnemyStateMachine.ChangeState<ChaseState>();
        }

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
           EnemyStateMachine.ChangeState<EnemyIdleState>();
        }
        
    }

  
    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz -= overlapSphereHandler.OnDrawGizmos;
    }
}

