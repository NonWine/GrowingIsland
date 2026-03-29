using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.Utilities;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class PlayerController
{
    private readonly IEntityAnimateable animateable;
    private readonly PlayerStateMachine playerStateMachine;
    private readonly IEnumerable<IPlayerDetector> detectors;

    public PlayerController(
      
        IEntityAnimateable animateable,
        PlayerStateMachine playerStateMachine,
        IEnumerable<IPlayerDetector> detectors)
    {
        this.animateable = animateable;
        this.playerStateMachine = playerStateMachine;
        this.detectors = detectors;
        detectors.ForEach( x => Debug.Log(x.GetType()));
        
    }

    public void Tick()
    {

        animateable.UpdateAnimator();
        playerStateMachine.CurrentState.LogicUpdate();

        foreach (var detector in detectors)
        {
            detector.Detect();
        }
    }
}

