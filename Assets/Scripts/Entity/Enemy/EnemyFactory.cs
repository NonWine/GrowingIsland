using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : PoolableFactory<EnemyType,BaseEnemy>
{
    public EnemyFactory(Dictionary<EnemyType, BaseEnemy> prefabs, DiContainer container) : base(prefabs, container)
    {
        
    }


}