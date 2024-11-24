

public class MoveState 
{
    private IMoveable _moveable;

    public MoveState(IMoveable moveable)
    {
        _moveable = moveable;
    }
    
    public void EnterState(BaseEnemy enemy)
    {
        
    }

    public void UpdateState()
    {
        _moveable.Move();
    }

    public void ExitState()
    {
        
    }
}