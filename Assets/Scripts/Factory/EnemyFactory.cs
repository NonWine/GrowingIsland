using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyFactory : IFactory<BaseEnemy>
{
    private DiContainer _diContainer;
    

    public  EnemyFactory( DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    public BaseEnemy Create(BaseEnemy baseEnemy)
    {
       return _diContainer.InstantiatePrefabForComponent<BaseEnemy>(baseEnemy);
    }

    public BaseEnemy Create(BaseEnemy Object, Transform transform, Quaternion rotation, Transform parent)
    {
        return _diContainer.InstantiatePrefabForComponent<BaseEnemy>(Object, transform.position, rotation, parent);
    }


}

public class Unit : MonoBehaviour
{
    
}
