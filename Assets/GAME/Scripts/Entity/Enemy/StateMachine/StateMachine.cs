using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class EnemyStateMachine
{
    [ShowInInspector] private EnemyState currentState;
    private Dictionary<Type, EnemyState> states = new Dictionary<Type, EnemyState>();
    private BaseEnemy baseEnemy;

    public BaseEnemy Enemy => baseEnemy;
    
    public EnemyState CurrentState => currentState;

    public EnemyStateMachine(BaseEnemy baseEnemy)
    {
        this.baseEnemy = baseEnemy;
    }

    private EnemyState GetState<T>() where T : EnemyState
    {
        states.TryGetValue(typeof(T), out var state);
        if (state == null)
        {
            Debug.LogError($"State of type {typeof(T)} not found!");
            return default;
        }
        return (T)state;
    }
    
    public void Initialize<T>(Dictionary<Type, EnemyState> states) where T : EnemyState
    {
        this.states = states;
        currentState = GetState<T>();
        currentState.EnterState(baseEnemy);
    }

    public void ChangeState<T>() where T : EnemyState
    {
        currentState.ExitState();
        currentState = GetState<T>();
        currentState.EnterState(baseEnemy);
    }

    public void Update()
    {
        currentState.UpdateState();
    }
    

}