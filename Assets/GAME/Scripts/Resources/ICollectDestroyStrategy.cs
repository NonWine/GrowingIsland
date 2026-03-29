using System;
using UnityEngine;

public interface ICollectDestroyStrategy
{
    void Collect(ResourcePartObj resource, Transform collector, Action onImpact = null);
}
