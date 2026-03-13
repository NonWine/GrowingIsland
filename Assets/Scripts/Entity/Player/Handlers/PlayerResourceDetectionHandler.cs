using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
            resource.transform.DOMove(playerContainer.transform.position, 0.25f).SetEase(Ease.InBack);
            Debug.Log("Picked up a resource!");
        }
    }

    public void NoDetection()
    {
    }
}

