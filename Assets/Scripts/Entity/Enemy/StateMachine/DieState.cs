using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class DieState : EnemyState
{

    private NavMeshAgent _agent;
    
    public DieState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(enemyStateMachine, enemyAnimator)
    {
        _agent = agent;
    }
    
    public override void EnterState( BaseEnemy baseEnemy)
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        EnemyAnimator.Die();
        Die();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        _agent.isStopped = false;

    }

    private async void Die()
    {
    
        await UniTask.Delay(4000);
        EnemyStateMachine.Enemy.gameObject.SetActive(false);

    }

} 