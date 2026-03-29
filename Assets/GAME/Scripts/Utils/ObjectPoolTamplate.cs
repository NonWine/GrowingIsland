using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPoolTemplate<TType, TEntity> where TEntity : PoolAble
{
    [Inject] private PoolableFactory<TType, TEntity> factory;
    public List<TEntity> _inActiveUnits = new List<TEntity>();

    
    // Очищення пулу
    public void ClearPool() => _inActiveUnits.Clear();

    // Метод для пошуку сутності, яка неактивна
    private TEntity TryResetFromPool()
    {
        foreach (var entity in _inActiveUnits)
        {
            if (!entity.gameObject.activeSelf)
            {
                entity.ResetPool();
                return entity;
            }
        }
        return null;
    }

    // Спавн об'єкта з пулу або створення нового через фабрику
    public TEntity SpawnEntity(TType entityType, Transform pos, Quaternion rotation)
    {
        var unit = TryResetFromPool();
        if (unit != null)
        {
            unit.transform.position = pos.position;
            unit.transform.rotation = rotation;
            unit.transform.localScale = new Vector3(1f, 1f, 1f);
            unit.gameObject.SetActive(true);
            return unit;
        }

        // Створення нового об'єкта через фабрику
        unit = factory.Create(entityType);
        unit.transform.position = pos.position;
        unit.transform.rotation = rotation;
        _inActiveUnits.Add(unit);
        
        return unit;
    }

    // Повернення об'єкта в пул
    public void ToPool(TEntity entity) 
    {
        entity.gameObject.SetActive(false);
       //          entity.ReturnToPool();
    }
}
