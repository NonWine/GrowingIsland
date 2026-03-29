using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class DieState : EnemyState
{

    private NavMeshAgent agent;
    
    public DieState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(enemyStateMachine, enemyAnimator)
    {
        this.agent = agent;
    }
    
    public override void EnterState( BaseEnemy baseEnemy)
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        EnemyAnimator.Die();
        Die();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        agent.isStopped = false;

    }

    private async void Die()
    {
    
        await UniTask.Delay(4000);
        Enemy.gameObject.SetActive(false);

    }

} 

