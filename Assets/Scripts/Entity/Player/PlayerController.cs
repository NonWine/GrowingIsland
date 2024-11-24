using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Zenject;

public class PlayerController 
{ 
    private IMoveable _moveable;
    private IRotateable _rotateable;
    private IEntityAnimateable _animateable;
    private PlayerStateMachine _playerStateMachine;
    private PlayerResourceDetector _playerResourceDetector;
    private PlayerHandlersService _playerHandlersService;
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerResourceDetector playerResourceDetector, 
        PlayerStateMachine playerStateMachine
        )
    {
        _playerResourceDetector = playerResourceDetector;
        _animateable = animateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
    }   
    
    public void Tick()
    {
       // _attackable.Attack();
        _moveable.Move();
        _rotateable.Rotate();
        _animateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();
        //maybe optimize it in future???
       _playerResourceDetector.FindResources();
    }
}