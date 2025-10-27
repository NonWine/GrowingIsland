using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private Dictionary<Type, EnemyState> _states = new Dictionary<Type, EnemyState>();
    private EnemyState _currentState;
    private BaseEnemy _base;

    public BaseEnemy Enemy => _base;

    public EnemyStateMachine(BaseEnemy baseEnemy)
    {
        _base = baseEnemy;
    }

    private EnemyState GetState<T>() where T : EnemyState
    {
        _states.TryGetValue(typeof(T), out var state);
        if (state == null)
        {
            Debug.LogError($"State of type {typeof(T)} not found!");
            return default;
        }
        return (T)state;
    }
    
    public void Initialize<T>(Dictionary<Type, EnemyState> states) where T : EnemyState
    {
        _states = states;
        _currentState = GetState<T>();
        _currentState.EnterState(_base);
    }

    public void ChangeState<T>() where T : EnemyState
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

public abstract class EnemyState
{
    protected EnemyStateMachine EnemyStateMachine;
    protected EnemyAnimator EnemyAnimator;

    public EnemyState(EnemyStateMachine enemyStateMachine, EnemyAnimator enemyAnimator )
    {
        EnemyStateMachine = enemyStateMachine;
        EnemyAnimator = enemyAnimator;
    }
    
    public abstract void EnterState(BaseEnemy baseEnemy);

    public abstract void UpdateState();

    public abstract void ExitState();

}