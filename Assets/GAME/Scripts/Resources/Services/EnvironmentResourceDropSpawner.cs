using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentResourceDropSpawner : IDropSpawner
{
    private const float ScatterRadius = 2f;
    private readonly ResourcePartObjFactory resourcesFactory;
    private ResourceWorld resourceWorld;

    public EnvironmentResourceDropSpawner(ResourcePartObjFactory resourcesFactory, ResourceWorld resourceWorld)
    {
        this.resourcesFactory = resourcesFactory;
        this.resourceWorld = resourceWorld;
    }

    public void Spawn(Vector3 origin)
    {
        for (int i = 0; i < resourceWorld.VisualDrop; i++)
        {
            var resource = resourcesFactory.Create(resourceWorld.TypeWallet);
            resource.transform.position = origin;

            var offset = origin + Random.insideUnitSphere * ScatterRadius;
            offset.y = resource.transform.position.y;

            resource.transform.DOMove(offset, 0.8f).SetEase(Ease.OutQuart);
            resource.ResetPool();
        }
    }

}

public interface IDropSpawner
{
    public void Spawn(Vector3 origin);

}