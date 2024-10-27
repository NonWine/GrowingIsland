using UnityEngine;

public interface IFactory<T>
{
    T Create(T baseEnemy);
    
    T Create(T Object, Transform transform, Quaternion rotation, Transform parent);
}