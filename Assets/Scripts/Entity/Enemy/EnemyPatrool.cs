using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPatrool : BaseEnemy
{
    [Inject] private PatrolArea _patroolArea;
    

    protected override Dictionary<Type, EnemyState> CreateStates()
    {
        var states = base.CreateStates(); // Викликаємо базовий метод і додаємо новий стан
        states[typeof(EnemyIdleState)] = new EnemyPatroolIdleState(EnemyAnimator, EnemyStateMachine);
        states[typeof(EnemyPatroolState)] = new EnemyPatroolState(EnemyAnimator, EnemyStateMachine, Player, _patroolArea, NavMesh);
        return states;
    }

}