using System;
using System.Collections.Generic;
using Entity.Enemy;
using UnityEngine;

public class EnemyPatrool : BaseEnemy
{
    [SerializeField] private PatroolArea _patroolArea;
    

    protected override Dictionary<Type, EnemyState> CreateStates()
    {
        var states = base.CreateStates(); // Викликаємо базовий метод і додаємо новий стан
        states[typeof(EnemyIdleState)] = new EnemyPatroolIdleState(EnemyAnimator, EnemyStateMachine, Player);
        states[typeof(EnemyPatroolState)] = new EnemyPatroolState(EnemyAnimator, EnemyStateMachine, Player, _patroolArea, NavMesh);
        return states;
    }

}