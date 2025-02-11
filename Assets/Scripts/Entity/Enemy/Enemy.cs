using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : BaseEnemy
{
    private EnemyAnimator _enemyAnimator;

    
    private void Start()
    {
        _enemyAnimator = new EnemyAnimator(Animator);
        Stats.CurrentHealth = Stats.MaxHealth;
        HealthUI.SetHealth(Stats.CurrentHealth);


        StateMachineInitialize();
    }

    private void StateMachineInitialize()
    {
        EnemyStateMachine = new EnemyStateMachine(this);
        EnemyIdleState idleState = new EnemyIdleState(_enemyAnimator,EnemyStateMachine, Player);
        MoveState moveState = new MoveState(EnemyStateMachine, _enemyAnimator,new EnemyMovingIdle( transform.position, NavMesh, Player.PlayerContainer, EnemyStateMachine));
        DieState dieState = new DieState(_enemyAnimator, EnemyStateMachine, NavMesh);
        ResetingState resetingState = new ResetingState(EnemyStateMachine, _enemyAnimator, NavMesh, Rigidbody, HealthUI);
        AttackState attackState = new AttackState(_enemyAnimator, EnemyStateMachine, new EnemyAttackIdle(Player, EnemyStateMachine));
        EnemyBackHomeState enemyBackHomeState =
            new EnemyBackHomeState(_enemyAnimator, EnemyStateMachine, Player, NavMesh, transform.position);
        Dictionary<Type, EnemyState> dictionary = new Dictionary<Type, EnemyState>()
        {
            { typeof(EnemyIdleState), idleState },
            { typeof(AttackState), attackState },
            { typeof(MoveState), moveState },
            { typeof(DieState), dieState },
            { typeof(ResetingState), resetingState },
            { typeof(EnemyBackHomeState), enemyBackHomeState }

        };

        EnemyStateMachine.Initialize<EnemyIdleState>(dictionary);
    }

    public override void ResetPool()
    {
       
    }
}