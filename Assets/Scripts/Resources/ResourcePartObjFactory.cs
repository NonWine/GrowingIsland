using System.Collections.Generic;
using Zenject;

public class ResourcePartObjFactory :  PoolableFactory<eCollectable,ResourcePartObj>
{
    public ResourcePartObjFactory(Dictionary<eCollectable, ResourcePartObj> prefabs, DiContainer container) : base(prefabs, container)
    {
        
    }
}