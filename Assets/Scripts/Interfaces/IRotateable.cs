using UnityEngine;

public interface IRotateable
{
    void Rotate();
}

public interface ITarget
{
    Transform Transform { get; }
}

public interface IGameTickable
{
    void Tick();
}

public interface IGameСontroller
{
    void RegisterInTick(IGameTickable gameTickable);

    void UnregisterFromTick(IGameTickable gameTickable);
}