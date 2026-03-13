using UnityEngine;

public class  EnemyPatroolIdleState : EnemyIdleState
{
    private float timer;
    
    public EnemyPatroolIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine) : base(enemyAnimator, enemyStateMachine)
    {
        
    }
    public override void EnterState(BaseEnemy baseEnemy)
    {
        base.EnterState(baseEnemy);
        timer = 0f;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            EnemyStateMachine.ChangeState<EnemyPatroolState>();
        }
    }
    
}
