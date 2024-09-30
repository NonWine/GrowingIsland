using UnityEngine;
using UnityEngine.AI;

public class ResetingState : IEnemyState
{
    private NavMeshAgent _meshAgent;
    private Rigidbody _rigidbody;
    private HealthUI _healthUI;
    
    public ResetingState(
        NavMeshAgent navMeshAgent,
        Rigidbody rigidbody,
        HealthUI healthUI)
    {
        _meshAgent = navMeshAgent;
        _rigidbody = rigidbody;
        _healthUI = healthUI;
    }    
    public void EnterState(BaseEnemy enemy)
    {
        _rigidbody.isKinematic = true;
        _meshAgent.enabled = true;
        enemy.gameObject.SetActive(true);
        enemy.Stats.CurrentHealth = enemy.Stats.MaxHealth;
        _healthUI.gameObject.SetActive(true);
        _healthUI.SetHealth(enemy.Stats.MaxHealth);
        _rigidbody.transform.localScale = new Vector3(1f,1f,1f);
        enemy.Stats.IsDeath = false;
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}