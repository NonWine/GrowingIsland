using UnityEngine;

public class  EnemyPatroolIdleState : EnemyIdleState
{
    private float _timer;
    
    public EnemyPatroolIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine) : base(enemyAnimator, enemyStateMachine)
    {
        
    }
    public override void EnterState(BaseEnemy baseEnemy)
    {
        base.EnterState(baseEnemy);
        _timer = 0f;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        _timer += Time.deltaTime;
        if (_timer >= 3f)
        {
            EnemyStateMachine.ChangeState<EnemyPatroolState>();
        }
    }
    
}