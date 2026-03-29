
using UnityEngine;

[System.Serializable]
public abstract class EnemyState
{
    protected EnemyStateMachine EnemyStateMachine;
    protected EnemyAnimator EnemyAnimator;

    protected BaseEnemy Enemy => EnemyStateMachine.Enemy;

    public EnemyState(EnemyStateMachine enemyStateMachine, EnemyAnimator enemyAnimator )
    {
        EnemyStateMachine = enemyStateMachine;
        EnemyAnimator = enemyAnimator;
    }
    
    public abstract void EnterState(BaseEnemy baseEnemy);

    public abstract void UpdateState();

    public abstract void ExitState();


}