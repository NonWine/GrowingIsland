using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

[System.Serializable]
public class WoodcutterStateMachine : ITickable, IInitializable, IDisposable
{
    private readonly Dictionary<Type, IState> states = new();
    private readonly SignalBus signalBus;

    [ShowInInspector] public IState CurrentState { get; private set; }

    public WoodcutterStateMachine(SignalBus signalBus, List<IState> states)
    {
        this.signalBus = signalBus;
        foreach (var state in states)
            this.states[state.GetType()] = state;
    }

    public void Initialize()
    {
        signalBus.Subscribe<ChangeWoodcutterStateSignal>(OnChangeStateSignal);
        ChangeState<WoodcutterIdleState>();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void Dispose()
    {
        CurrentState?.Exit();
        signalBus.TryUnsubscribe<ChangeWoodcutterStateSignal>(OnChangeStateSignal);
    }

    private void OnChangeStateSignal(ChangeWoodcutterStateSignal signal)
    {
        ChangeState(signal.TargetStateType);
    }

    public void ChangeState<T>() where T : IState => ChangeState(typeof(T));

    private void ChangeState(Type stateType)
    {
        if (!states.TryGetValue(stateType, out var next))
        {
            UnityEngine.Debug.LogError($"State {stateType} not registered!");
            return;
        }

        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }
}
