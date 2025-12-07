using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetectorBase<TTarget> : IPlayerDetector
{
    protected readonly PlayerContainer PlayerContainer;
    protected readonly OverlapSphereHandler OverlapSphereHandler;
    private readonly IDetectionHandler<TTarget> _detectionHandler;

    protected PlayerDetectorBase(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        IDetectionHandler<TTarget> detectionHandler)
    {
        PlayerContainer = playerContainer;
        OverlapSphereHandler = overlapSphereHandler;
        _detectionHandler = detectionHandler;
    }

    public void Detect()
    {
        if (!CanDetect())
        {
            return;
        }

        var detectedObjects = OverlapSphereHandler.GetFilteredObjects<TTarget>(
            PlayerContainer.transform.position,
            GetRadius(),
            GetLayerMask(),
            GetFilter(),
            ShouldForceUpdate());

        if (detectedObjects.Count == 0)
        {
            return;
        }

        OnDetected(detectedObjects);
    }

    protected abstract float GetRadius();
    protected virtual void OnDetected(List<TTarget> detectedObjects)
    {
        _detectionHandler?.Handle(detectedObjects);
    }

    protected virtual bool CanDetect() => true;
    protected virtual LayerMask GetLayerMask() => 0;
    protected virtual bool ShouldForceUpdate() => false;
    protected virtual Func<TTarget, bool> GetFilter() => null;
}
