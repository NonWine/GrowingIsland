using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.Utilities;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class PlayerController
{
    private readonly IMoveable _moveable;
    private readonly IRotateable _rotateable;
    private readonly IEntityAnimateable _animateable;
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly IEnumerable<IPlayerDetector> _detectors;

    public PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerStateMachine playerStateMachine,
        IEnumerable<IPlayerDetector> detectors)
    {
        _animateable = animateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
        _detectors = detectors;
        detectors.ForEach( x => Debug.Log(x.GetType()));
        
    }

    public void Tick()
    {
        _moveable.Move();
        _rotateable.Rotate();
        _animateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();

        foreach (var detector in _detectors)
        {
            detector.Detect();
        }
    }
}
