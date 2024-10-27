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

    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable entityAnimateable,
        IAttackable attackable,
        PlayerStateMachine playerStateMachine
        )
{
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
    }
}