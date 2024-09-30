using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPoolEnemy
{
    [Inject] private IFactory<BaseEnemy> _unitFactory;
    public  List<BaseEnemy> _InActiveUnits = new List<BaseEnemy>();

    public  void ClearPool() => _InActiveUnits.Clear();
    
    private BaseEnemy TryResetFromPool()
    {
        foreach (var enemy in _InActiveUnits)
        {
            if (enemy.gameObject.activeSelf == false)
            {
                enemy.ResetMob();
                return enemy;
            }
        }

        return null;
    }
    

    
    public BaseEnemy SpawnEnemy(BaseEnemy baseEnemy, Transform pos, Quaternion rotation)
    {
        var NewUnit = TryResetFromPool();
        if (NewUnit != null)
        {
            NewUnit.transform.position = pos.position;
            NewUnit.transform.rotation = rotation;
            NewUnit.transform.localScale = new Vector3(1f, 1f, 1f);
            return NewUnit;
        }

        NewUnit = _unitFactory.Create(baseEnemy, pos, rotation, null);
        _InActiveUnits.Add(NewUnit);

        NewUnit.ObjectPoolEnemy = this;
        return NewUnit;
    }

    public void ToPool(BaseEnemy unit) 
    {
        unit.gameObject.SetActive(false);
    }
}