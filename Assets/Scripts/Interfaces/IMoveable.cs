public interface IMoveable
{
    void Move();
}

public interface IEnemyMoveable : IMoveable
{
    void StartMove();
}