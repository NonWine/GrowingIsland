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
    private PlayerFarmDetector _playerFarmDetector;
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerResourceDetector playerResourceDetector, 
        PlayerStateMachine playerStateMachine,
        PlayerFarmDetector playerFarmDetector
        )
    {
        _playerResourceDetector = playerResourceDetector;
        _animateable = animateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
        _playerFarmDetector = playerFarmDetector;
    }   
    
    public void Tick()
    {
       // _attackable.Attack();
        _moveable.Move();
        _rotateable.Rotate();
        _animateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();

        _playerResourceDetector.FindResources();
       _playerFarmDetector.FindFarmingResources();
    }
}