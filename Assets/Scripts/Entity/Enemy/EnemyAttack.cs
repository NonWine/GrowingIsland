using UnityEngine;

public abstract class EnemyAttack : IAttackable
{
    
    protected Player Player;
    protected EnemyStateMachine EnemyStateMachine;
    protected float Timer;

    protected BaseEnemy Enemy => EnemyStateMachine.Enemy;

    public EnemyAttack(Player player, EnemyStateMachine enemyStateMachine)
    {
        Player = player;
        EnemyStateMachine = enemyStateMachine;
    }

    public void Attack()
    {
        Timer += Time.deltaTime;

        if (Timer >= Enemy.Stats.TimeBeetwenHit)
        {
            Timer = 0f;
            Player.GetDamage(Enemy.Stats.DamageHit);
             
        }

        if (Vector3.Distance(Player.transform.position, Enemy.transform.position) > Enemy.Stats.TargetDistance)
        {
            EnemyStateMachine.ChangeState<MoveState>();
        }
        
    }
}