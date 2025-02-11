
public class PlayerController 
{ 
    private IMoveable _moveable;
    private IRotateable _rotateable;
    private IEntityAnimateable _animateable;
    private PlayerStateMachine _playerStateMachine;
    private PlayerResourceDetector _playerResourceDetector;
    private PlayerFarmDetector _playerFarmDetector;
    private PlayerEnemyDetector _playerEnemyDetector;
    
    public  PlayerController(IMoveable moveable,
        IRotateable rotateable,
        IEntityAnimateable animateable,
        PlayerResourceDetector playerResourceDetector, 
        PlayerStateMachine playerStateMachine,
        PlayerFarmDetector playerFarmDetector,
        PlayerEnemyDetector playerEnemyDetector
        )
    {
        _playerResourceDetector = playerResourceDetector;
        _animateable = animateable;
        _moveable = moveable;
        _rotateable = rotateable;
        _playerStateMachine = playerStateMachine;
        _playerFarmDetector = playerFarmDetector;
        _playerEnemyDetector = playerEnemyDetector;
    }   
    
    public void Tick()
    {
        _moveable.Move();
        _rotateable.Rotate();
        _animateable.UpdateAnimator();
        _playerStateMachine.CurrentState.LogicUpdate();
        _playerEnemyDetector.Find();
    //    _playerResourceDetector.FindResources();
    //   _playerFarmDetector.FindFarmingResources();
    }
}