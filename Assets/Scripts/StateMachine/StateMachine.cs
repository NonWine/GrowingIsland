using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private readonly Dictionary<Type, IEnemyState> _states = new Dictionary<Type, IEnemyState>();
    private IEnemyState _currentState;
    private BaseEnemy _base;

    public EnemyStateMachine(BaseEnemy baseEnemy, Dictionary<Type, IEnemyState> states)
    {
        _states = states;
        _base = baseEnemy;
    }

    private IEnemyState GetState<T>() where T : IEnemyState
    {
        _states.TryGetValue(typeof(T), out var state);
        if (state == null)
        {
            Debug.LogError($"State of type {typeof(T)} not found!");
            return default;
        }
        return (T)state;
    }
    
    public void Initialize<T>() where T : IEnemyState
    {
        _currentState = GetState<T>();
        _currentState.EnterState(_base);
    }

    public void ChangeState<T>() where T : IEnemyState
    {
        _currentState.ExitState();
        _currentState = GetState<T>();
        _currentState.EnterState(_base);
    }

    public void Update()
    {
        _currentState.UpdateState();
    }
    
    
}