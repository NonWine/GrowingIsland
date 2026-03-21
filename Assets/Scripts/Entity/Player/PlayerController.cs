using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.Utilities;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class PlayerController
{
    private readonly IMoveable moveable;
    private readonly IRotateable rotateable;
    private readonly IEntityAnimateable animateable;
    private readonly PlayerStateMachine playerStateMachine;
    private readonly IEnumerable<IPlayerDetector> detectors;

    public PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerStateMachine playerStateMachine,
        IEnumerable<IPlayerDetector> detectors)
    {
        this.animateable = animateable;
        this.moveable = moveable;
        this.rotateable = rotateable;
        this.playerStateMachine = playerStateMachine;
        this.detectors = detectors;
        detectors.ForEach( x => Debug.Log(x.GetType()));
        
    }

    public void Tick()
    {
        moveable.Move();
        rotateable.Rotate();
        animateable.UpdateAnimator();
        playerStateMachine.CurrentState.LogicUpdate();

        foreach (var detector in detectors)
        {
            detector.Detect();
        }
    }
}

