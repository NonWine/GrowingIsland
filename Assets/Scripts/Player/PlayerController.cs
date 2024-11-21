using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerController 
{ 
    private IMoveable _moveable;
    private IRotateable _rotateable;
    private IEntityAnimateable _entityAnimateable;
    private IAttackable _attackable;
    private PlayerStateMachine _playerStateMachine;
    private ResourceDetector _resourceDetector;
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable entityAnimateable,
        IAttackable attackable,
        PlayerStateMachine playerStateMachine,
        ResourceDetector resourceDetector
        )
    {
        _resourceDetector = resourceDetector;
        _attackable = attackable;
        _entityAnimateable = entityAnimateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
}

    public void Tick()
    {
       // _attackable.Attack();
        _moveable.Move();
        _rotateable.Rotate();
        _entityAnimateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();
        //maybe optimize it in future???
       _resourceDetector.FindResources();
    }
}

public class ResourceDetector
{
    private PlayerContainer _playerContainer;
    private Collider[] _overlapResults;

    public ResourceDetector(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
        _overlapResults = new Collider[20];
    }

    public void FindResources()
    {
        int count = Physics.OverlapSphereNonAlloc(
            _playerContainer.transform.position,
            _playerContainer.PlayerStats.RadiusDetection,
            _overlapResults
        );

        for (int i = 0; i < count; i++)
        {
            if (_overlapResults[i].TryGetComponent(out ResourcePartObj resource))
            {
                resource.PickUp();
            }
        }

    }
}