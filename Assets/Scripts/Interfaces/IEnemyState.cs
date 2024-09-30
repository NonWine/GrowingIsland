public interface IEnemyState
{
    void EnterState(BaseEnemy enemy);
    void UpdateState();
    void ExitState();
}