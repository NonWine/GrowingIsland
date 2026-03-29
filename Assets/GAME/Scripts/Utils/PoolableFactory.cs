using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PoolableFactory<TType, TEntity> : IFactory<TType, TEntity>
    where TEntity : PoolAble
{
    private readonly Dictionary<TType, TEntity> prefabs;
    private readonly DiContainer container;

    public PoolableFactory(Dictionary<TType, TEntity> prefabs, DiContainer container)
    {
        this.prefabs = prefabs;
        this.container = container;
    }

    public TEntity Create(TType type)
    {
        if (!prefabs.ContainsKey(type))
        {
            Debug.LogError($"Prefab for {type} not found!");
            return null;
        }

        return Create(prefabs[type]);
    }

    public TEntity Create(TEntity prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab for {typeof(TEntity).Name} is null!");
            return null;
        }

        return container.InstantiatePrefabForComponent<TEntity>(prefab);
    }
}

