using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class PoolableFactory<TType, TEntity> : IFactory<TType, TEntity>
    where TEntity : PoolAble
{
    private Dictionary<TType, TEntity> _prefabs;
    private DiContainer _container;

    public PoolableFactory(Dictionary<TType, TEntity> prefabs, DiContainer container)
    {
        _prefabs = prefabs;
        _container = container;
    }

    // Метод для створення нового об'єкта або витягування його з пулу
    public TEntity Create(TType type)
    {
        if (!_prefabs.ContainsKey(type))
        {
            Debug.LogError($"Prefab for {type} not found!");
            return null;
        }

        var prefab = _prefabs[type];
        var instance = _container.InstantiatePrefabForComponent<TEntity>(prefab);
        // instance.gameObject.SetActive(true);
        //instance.OnSpawn();
        return instance;
    }

}