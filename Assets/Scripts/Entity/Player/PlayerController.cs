using System.Collections.Generic;

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
