using System.Collections.Generic;

public class PlayerStateMachine
{
    private Dictionary<PlayerStateKey, PlayerState> _playerStates;
    public PlayerState CurrentState { get; private set; }
    
    public PlayerStateKey CurrentStateKey { get; private set; }

    public void RegisterStates(Dictionary<PlayerStateKey, PlayerState> states) => _playerStates = states;
    
    public void Initialize(PlayerStateKey startingState)
    {
        ChangeState(startingState);
    }

    public void ChangeState(PlayerStateKey state)
    {
        if(CurrentState != null)
            CurrentState.Exit();
        CurrentState = GetState(state);
        CurrentState.Enter();
        CurrentStateKey = state;
    }

    private PlayerState GetState(PlayerStateKey playerStateKey)
    {
        _playerStates.TryGetValue(playerStateKey, out PlayerState playerState);
        return playerState;
    }
    
}
