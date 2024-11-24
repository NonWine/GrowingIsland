using System;
using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;
    private PlayerStateMachine _playerStateMachine;

    public event Action<eCollectable> CurrentResourceTrigger;

    private void Awake()
    {
        _playerStateMachine = _player.PlayerStateMachine;
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