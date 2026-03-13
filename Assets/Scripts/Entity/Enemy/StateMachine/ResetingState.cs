using UnityEngine;
using UnityEngine.AI;

public class ResetingState : EnemyState
{
    private NavMeshAgent meshAgent;
    private Rigidbody rigidbody;
    private HealthUI healthUI;
    
    public ResetingState(
        EnemyStateMachine enemyStateMachine,
        EnemyAnimator enemyAnimator,
        NavMeshAgent navMeshAgent,
        Rigidbody rigidbody,
        HealthUI healthUI) : base(enemyStateMachine, enemyAnimator)
    {
        meshAgent = navMeshAgent;
        this.rigidbody = rigidbody;
        this.healthUI = healthUI;
    }    
    public override void EnterState(BaseEnemy enemy)
    {
        rigidbody.isKinematic = true;
        meshAgent.enabled = true;
        enemy.gameObject.SetActive(true);
        enemy.Stats.CurrentHealth = enemy.Stats.MaxHealth;
        healthUI.gameObject.SetActive(true);
        healthUI.SetHealth(enemy.Stats.MaxHealth);
        rigidbody.transform.localScale = new Vector3(1f,1f,1f);
        enemy.IsDeath = false;
        
        EnemyStateMachine.ChangeState<EnemyIdleState>();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
}

