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
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable entityAnimateable,
        IAttackable attackable
        )
{
        _attackable = attackable;
        _entityAnimateable = entityAnimateable;
        _moveable = moveable;
        _rotateable = rotateable;
    }

    public void Tick()
    {
        _attackable.Attack();
        _moveable.Move();
        _rotateable.Rotate();
        _entityAnimateable.UpdateAnimator();
    }
}