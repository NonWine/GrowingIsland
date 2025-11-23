using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolArea : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    
    public Vector3 GetRandomPointInCollider()
    {
       return ColliderUtils.GetRandomPointInsideSphere(sphereCollider);
    }
}

public static class ColliderUtils
{
    public static Vector3 GetRandomPointInsideSphere(SphereCollider sphere)
    {
        Vector3 randomPoint = Random.insideUnitSphere;

        float scaledRadius = sphere.radius * Mathf.Max(
            sphere.transform.lossyScale.x,
            sphere.transform.lossyScale.y,
            sphere.transform.lossyScale.z
        );

        Vector3 worldPoint = sphere.transform.TransformPoint(randomPoint * scaledRadius);

        return worldPoint;
    }
}