using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PoolableFactory<TType, TEntity> : IFactory<TType, TEntity>
    where TEntity : PoolAble
{
    private Dictionary<TType, TEntity> prefabs;
    private DiContainer container;

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

        var prefab = prefabs[type];
        var instance = container.InstantiatePrefabForComponent<TEntity>(prefab);
        // instance.gameObject.SetActive(true);
        //instance.OnSpawn();
        return instance;
    }

}

