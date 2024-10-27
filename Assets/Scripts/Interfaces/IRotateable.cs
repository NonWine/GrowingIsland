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