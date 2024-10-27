using System.Collections;
using UnityEngine;
using Zenject;

public class ResourceFactory : IFactory<Resource>
{
    private DiContainer _diContainer;
    

    public  ResourceFactory( DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    public Resource Create(Resource baseEnemy)
    {
        
       return _diContainer.InstantiatePrefabForComponent<Resource>(baseEnemy);
    }
    
    public Resource Create(Resource Object, Transform transform, Quaternion rotation, Transform parent)
    {
        return _diContainer.InstantiatePrefabForComponent<Resource>(Object, transform.position, rotation, parent);
    }


}