using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : BaseEnemy
{
    private void Start()
    {
        Stats.CurrentHealth = Stats.MaxHealth;
        HealthUI.SetHealth(Stats.CurrentHealth);
        EnemyStateMachine = new EnemyStateMachine(this, new Dictionary<Type, IEnemyState>() 
        {
            { typeof(MoveState), new MoveState(new EnemyMoving(NavMesh, 3f)) },
            { typeof(DieState), new DieState( NavMesh, Rigidbody, PlayerContainer) },
            { typeof(ResetingState), new ResetingState(NavMesh,Rigidbody, HealthUI ) }
        });
        EnemyStateMachine.Initialize<MoveState>();
    }
    
    public override void ResetMob()
    {
        EnemyStateMachine.ChangeState<ResetingState>();
        EnemyStateMachine.ChangeState<MoveState>();

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerContainer>() != null && Stats.IsDeath)
        {
            ParticlePool.Instance.PlayPoof(transform.position);
            ObjectPoolEnemy.ToPool(this);
        }
    }
}