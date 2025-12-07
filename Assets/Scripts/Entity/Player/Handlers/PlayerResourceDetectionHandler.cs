using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerResourceDetectionHandler : IDetectionHandler<ResourcePartObj>
{
    private readonly PlayerContainer _playerContainer;

    public PlayerResourceDetectionHandler(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
    }

    public void Handle(List<ResourcePartObj> resources)
    {
        foreach (var resource in resources)
        {
            resource.PickUp();
            resource.transform.DOMove(_playerContainer.transform.position, 0.25f).SetEase(Ease.InBack);
            Debug.Log("Picked up a resource!");
        }
    }
}
