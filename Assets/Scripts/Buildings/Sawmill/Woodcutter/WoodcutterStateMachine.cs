using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum WoodcutterStateKey
{
    Idle,
    SearchTree,
    MoveToTree,
    ChopTree,
    CollectDrops,
    ReturnToSawmill,
    WaitingStorage
}

[System.Serializable]
public class WoodcutterStateMachine
{
    private readonly Dictionary<WoodcutterStateKey, WoodcutterState> _states = new();

    [ShowInInspector] public WoodcutterState CurrentState { get; private set; }
    public WoodcutterStateKey CurrentKey { get; private set; }

    public static WoodcutterStateMachine CreateDefault(WoodcutterContext context)
    {
        var machine = new WoodcutterStateMachine();

        machine.Register(WoodcutterStateKey.Idle, new WoodcutterIdleState(context, machine));
        machine.Register(WoodcutterStateKey.SearchTree, new WoodcutterSearchTreeState(context, machine));
        machine.Register(WoodcutterStateKey.MoveToTree, new WoodcutterMoveToTreeState(context, machine));
        machine.Register(WoodcutterStateKey.ChopTree, new WoodcutterChopState(context, machine));
        machine.Register(WoodcutterStateKey.CollectDrops, new WoodcutterCollectState(context, machine));
        machine.Register(WoodcutterStateKey.ReturnToSawmill, new WoodcutterReturnState(context, machine));
        machine.Register(WoodcutterStateKey.WaitingStorage, new WoodcutterWaitingState(context, machine));

        machine.ChangeState(WoodcutterStateKey.Idle);

        return machine;
    }

    public void Register(WoodcutterStateKey key, WoodcutterState state)
    {
        _states[key] = state;
    }

    public void ChangeState(WoodcutterStateKey key)
    {
        if (!_states.TryGetValue(key, out var next))
        {
            Debug.LogError($"Woodcutter state {key} not found");
            return;
        }
        Debug.Log(next + "next State");
        CurrentState?.Exit();
        CurrentState = next;
        CurrentKey = key;
        CurrentState.Enter();
    }

    public void Tick()
    {
        CurrentState?.Tick();
    }
}