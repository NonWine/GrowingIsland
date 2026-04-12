using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStateMachine
{
    [ShowInInspector] private Dictionary<PlayerStateKey, PlayerState> playerStates;
    
    [ShowInInspector] public PlayerState CurrentState { get; private set; }
    
    public PlayerStateKey CurrentStateKey { get; private set; }

    public void RegisterStates(Dictionary<PlayerStateKey, PlayerState> states) => playerStates = states;
    
    public void Initialize(PlayerStateKey startingState)
    {
        ChangeState(startingState);
    }

    public void ChangeState(PlayerStateKey state)
    {
        if (CurrentState != null && CurrentStateKey == state)
        {
            return;
        }
        Debug.Log("new State is "  + state);
        if(CurrentState != null)
            CurrentState.Exit();
        CurrentStateKey = state;
        CurrentState = GetState(state);
        CurrentState.Enter();
    }

    private PlayerState GetState(PlayerStateKey playerStateKey)
    {
        playerStates.TryGetValue(playerStateKey, out PlayerState playerState);
        return playerState;
    }
    
}
