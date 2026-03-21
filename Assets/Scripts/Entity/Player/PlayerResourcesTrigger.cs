using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class PlayerResourcesTrigger : PlayerTrigger
{
    
    [Inject] private Player player;

    public event Action<eCollectable> CurrentResourceTrigger;

    protected  void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.TryGetComponent(out EnvironmentPropObjectView environmentResource))
        {
            player.PlayerStateMachine.ChangeState(PlayerStateKey.Farming);
            CurrentResourceTrigger?.Invoke(environmentResource.ResourceType);
        }

    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.TryGetComponent(out EnvironmentPropObjectView environmentResource))
        {
            player.PlayerStateMachine.ChangeState(PlayerStateKey.Idle);
        }
    }
}
