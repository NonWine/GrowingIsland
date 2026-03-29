using System.Collections.Generic;

public class PlayerResourceDetectionHandler : IDetectionHandler<ResourcePartObj>
{
    private readonly PlayerContainer playerContainer;

    public PlayerResourceDetectionHandler(PlayerContainer playerContainer)
    {
        this.playerContainer = playerContainer;
    }

    public void Handle(List<ResourcePartObj> resources)
    {
        foreach (var resource in resources)
        {
            resource.PickUp(playerContainer.Body, CollectStrategyType.Player);
        }
    }

    public void NoDetection()
    {
    }
}
