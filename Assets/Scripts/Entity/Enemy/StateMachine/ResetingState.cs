using UnityEngine;
using UnityEngine.AI;

public class ResetingState : EnemyState
{
    private NavMeshAgent _meshAgent;
    private Rigidbody _rigidbody;
    private HealthUI _healthUI;
    
    public ResetingState(
        EnemyStateMachine enemyStateMachine,
        EnemyAnimator enemyAnimator,
        NavMeshAgent navMeshAgent,
        Rigidbody rigidbody,
        HealthUI healthUI) : base(enemyStateMachine, enemyAnimator)
    {
        _meshAgent = navMeshAgent;
        _rigidbody = rigidbody;
        _healthUI = healthUI;
    }    
    public override void EnterState(BaseEnemy enemy)
    {
        _rigidbody.isKinematic = true;
        _meshAgent.enabled = true;
        enemy.gameObject.SetActive(true);
        enemy.Stats.CurrentHealth = enemy.Stats.MaxHealth;
        _healthUI.gameObject.SetActive(true);
        _healthUI.SetHealth(enemy.Stats.MaxHealth);
        _rigidbody.transform.localScale = new Vector3(1f,1f,1f);
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