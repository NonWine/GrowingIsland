using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected Player _player;
    private OverlapSphereHandler _overlapSphereHandler;
    private EnemyAnimator _enemyAnimator;
    
    public EnemyIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, Player player) : base(enemyStateMachine, enemyAnimator)
    {
        _enemyAnimator = enemyAnimator;
        _player = player;
        _overlapSphereHandler = new OverlapSphereHandler();
    }
    
    public override void EnterState(BaseEnemy baseEnemy)
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz += _overlapSphereHandler.OnDrawGizmos;
        _enemyAnimator.Idle();
    }

    public override void UpdateState()
    {
        if(Vector3.Distance(_player.transform.position, EnemyStateMachine.Enemy.transform.position) <= EnemyStateMachine.Enemy.Stats.RadiusDetection)
        {
            EnemyStateMachine.ChangeState<MoveState>();
        }
    }

  
    public override void ExitState()
    {
        EnemyStateMachine.Enemy.OnDrawGizmoz -= _overlapSphereHandler.OnDrawGizmos;
    }
}

public class  EnemyPatroolIdleState : EnemyIdleState
{
    private float _timer;
    
    public EnemyPatroolIdleState(EnemyAnimator enemyAnimator, EnemyStateMachine enemyStateMachine, Player player) : base(enemyAnimator, enemyStateMachine, player)
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