using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;

    public void Init(PlayerStateMachine playerStateMachine)
    {
        _playerStateMachine = playerStateMachine;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnvironmentResource environmentResource))
        {
            _playerStateMachine.ChangeState(PlayerStateKey.Lumber);
        }
        else if(other.TryGetComponent(out IPlayerEnterTriggable playerEnterTriggable))
            playerEnterTriggable.PlayerEnter();
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnvironmentResource>(out EnvironmentResource environmentResource))
        {
            _playerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
        else if(other.TryGetComponent(out IPlayerExitTriggable playerEnterTriggable))
            playerEnterTriggable.PlayerExit();
    }
}