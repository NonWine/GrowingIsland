using UnityEngine;

public interface IFactory<T>
{
    T Create(BaseEnemy baseEnemy);
    
    T Create(T Object, Transform transform, Quaternion rotation, Transform parent);
}