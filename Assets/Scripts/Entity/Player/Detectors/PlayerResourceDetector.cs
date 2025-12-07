using System;

public class PlayerResourceDetector : PlayerDetectorBase<ResourcePartObj>
{
    private static readonly Func<ResourcePartObj, bool> ResourceIsAvailable = resource => !resource.IsPicked;

    public PlayerResourceDetector(PlayerContainer playerContainer,
        OverlapSphereHandler overlapSphereHandler,
        IDetectionHandler<ResourcePartObj> detectionHandler)
        : base(playerContainer, overlapSphereHandler, detectionHandler)
    {
    }

    public void FindResources() => Detect();

    protected override float GetRadius() => PlayerContainer.PlayerStats.AggroRadius;

    protected override Func<ResourcePartObj, bool> GetFilter() => ResourceIsAvailable;
}
