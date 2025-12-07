
public class PlayerController 
{ 
    private IMoveable _moveable;
    private IRotateable _rotateable;
    private IEntityAnimateable _animateable;
    private PlayerStateMachine _playerStateMachine;
    private IPlayerDetector[] detectors;
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerStateMachine playerStateMachine,
        IPlayerDetector[] detectors
        )
    {
        _animateable = animateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
        this.detectors = detectors;
    }   
    
    public void Tick()
    {
        _moveable.Move();
        _rotateable.Rotate();
        _animateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();
        
        foreach (var item in detectors)
        {
            item.Detect();   
        }
    }
}