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

public class ResourcePartObjFactory :  PoolableFactory<eCollectable,ResourcePartObj>
{
    public ResourcePartObjFactory(Dictionary<eCollectable, ResourcePartObj> prefabs, DiContainer container) : base(prefabs, container)
    {
        
    }
}
