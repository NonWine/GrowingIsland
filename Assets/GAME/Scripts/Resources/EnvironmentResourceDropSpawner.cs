using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EnvironmentResourceDropSpawner
{
    private readonly ResourcePartObjFactory resourcesFactory;

    public EnvironmentResourceDropSpawner(ResourcePartObjFactory resourcesFactory)
    {
        this.resourcesFactory = resourcesFactory;
    }

    public void Spawn(eCollectable resourceType, int visualDropCount, Vector3 origin, float scatterRadius = 2f)
    {
        for (int i = 0; i < visualDropCount; i++)
        {
            var resource = resourcesFactory.Create(resourceType);
            resource.transform.position = origin;

            var offset = origin + Random.insideUnitSphere * scatterRadius;
            offset.y = resource.transform.position.y;

            resource.transform.DOMove(offset, 0.8f).SetEase(Ease.OutQuart);
            resource.ResetPool();
        }
    }
}
