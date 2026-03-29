public interface IGameController
{
    void RegisterInTick(IGameTickable gameTickable);
    void UnregisterFromTick(IGameTickable gameTickable);
}
