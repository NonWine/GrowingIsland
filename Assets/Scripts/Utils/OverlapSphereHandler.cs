using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSphereHandler
{
    private Collider[] overlapResults;
    private Vector3 lastPosition;
    private LayerMask lastLayerMask;
    private int lastCount;
    private float lastRadius;
    private bool resultsAreStale;

    private List<Collider> debugColliders = new List<Collider>();

    public OverlapSphereHandler(int maxColliders = 40)
    {
        overlapResults = new Collider[maxColliders];
        resultsAreStale = true;
    }

    /// <summary>
    /// Оновлює OverlapSphere вручну (опціонально).
    /// </summary>
    private void UpdateOverlapSphere(Vector3 position, float radius, LayerMask layerMask)
    {
        lastPosition = position;
        lastRadius = radius;
        lastLayerMask = layerMask;

        lastCount = Physics.OverlapSphereNonAlloc(position, radius, overlapResults, layerMask);
        resultsAreStale = false;

        // Оновлення для дебагу
        debugColliders.Clear();
        for (int i = 0; i < lastCount; i++)
        {
            debugColliders.Add(overlapResults[i]);
        }
    }

    /// <summary>
    /// Повертає всі об'єкти, які відповідають умовам.
    /// Якщо результати застарілі, виконує OverlapSphere автоматично.
    /// </summary>
    public List<T> GetFilteredObjects<T>(Vector3 position, float radius, LayerMask layerMask, Func<T, bool> filter = null, bool alwaysUpdate = false) 
    { 
        if(layerMask == 0)
            layerMask = LayerMask.GetMask("Default");
        
        if (resultsAreStale || position != lastPosition || radius != lastRadius || layerMask != lastLayerMask || alwaysUpdate)
        {
            UpdateOverlapSphere(position, radius, layerMask);
        }

        
        List<T> results = new List<T>();
        for (int i = 0; i < lastCount; i++)
        {
            if (overlapResults[i].TryGetComponent(out T component))
            {
                if (filter == null || filter(component))
                {
                    results.Add(component);
                }
            }
        }
        return results;
    }

    /// <summary>
    /// Малювання Gizmos для знайдених об'єктів.
    /// </summary>
    public void OnDrawGizmos()
    {
        // Малювання сфери Overlap
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(lastPosition, lastRadius);

        // Виділення знайдених об'єктів
        Gizmos.color = Color.yellow;
        foreach (var collider in debugColliders)
        {
            if (collider != null)
            {
                Gizmos.DrawSphere(collider.transform.position, 0.3f);
            }
        }
    }
}
