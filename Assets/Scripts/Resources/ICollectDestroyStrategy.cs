using UnityEngine;

public interface ICollectDestroyStrategy
{
    void Collect(Transform resource, Transform collector);
}