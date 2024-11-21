using System;
using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;

    public event Action<eCollectable> CurrentResourceTrigger; 

    public void Init(PlayerStateMachine playerStateMachine)
    {
        _playerStateMachine = playerStateMachine;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnvironmentResource environmentResource))
        {
            _playerStateMachine.ChangeState(PlayerStateKey.Lumber);
            CurrentResourceTrigger?.Invoke(environmentResource.ResourceType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnvironmentResource environmentResource))
        {
            _playerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }
}