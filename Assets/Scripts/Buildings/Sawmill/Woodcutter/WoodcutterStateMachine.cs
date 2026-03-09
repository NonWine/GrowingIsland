using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Zenject;

public class WoodcutterStateMachine : ITickable, IInitializable, IDisposable
{
    private readonly Dictionary<Type, WoodcutterState> _states = new();

    [ShowInInspector] public WoodcutterState CurrentState { get; private set; }

    public WoodcutterStateMachine(List<WoodcutterState> states)
    {
        foreach (var state in states)
            _states[state.GetType()] = state;
    }

    public void Initialize()
    {
        ChangeState<WoodcutterIdleState>();
    }

    public void ChangeState<T>() where T : WoodcutterState
    {
        var type = typeof(T);
        if (!_states.TryGetValue(type, out var next))
        {
            UnityEngine.Debug.LogError($"State {type} not registered!");
            return;
        }

        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void Dispose()
    {
        CurrentState?.Exit();
    }
}
