using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : PoolableFactory<Type,BaseEnemy>
{
    public EnemyFactory(Dictionary<Type, BaseEnemy> prefabs, DiContainer container) : base(prefabs, container)
    {
        
    }


}