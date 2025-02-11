using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSphereHandler
{
       private Collider[] _overlapResults;
    private int _lastCount;
    private Vector3 _lastPosition;
    private float _lastRadius;
    private LayerMask _lastLayerMask;
    private bool _resultsAreStale;

    private List<Collider> _debugColliders = new List<Collider>();

    public OverlapSphereHandler(int maxColliders = 20)
    {
        _overlapResults = new Collider[maxColliders];
        _resultsAreStale = true;
    }

    /// <summary>
    /// Оновлює OverlapSphere вручну (опціонально).
    /// </summary>
    private void UpdateOverlapSphere(Vector3 position, float radius, LayerMask layerMask)
    {
        _lastPosition = position;
        _lastRadius = radius;
        _lastLayerMask = layerMask;

        _lastCount = Physics.OverlapSphereNonAlloc(position, radius, _overlapResults, layerMask);
        _resultsAreStale = false;

        // Оновлення для дебагу
        _debugColliders.Clear();
        for (int i = 0; i < _lastCount; i++)
        {
            _debugColliders.Add(_overlapResults[i]);
        }
    }

    /// <summary>
    /// Повертає всі об'єкти, які відповідають умовам.
    /// Якщо результати застарілі, виконує OverlapSphere автоматично.
    /// </summary>
    public List<T> GetFilteredObjects<T>(Vector3 position, float radius, LayerMask layerMask, Func<T, bool> filter = null, bool alwaysUpdate = false) 
    { 
        //if(layerMask == 0)
            layerMask = LayerMask.GetMask("Default");
        
        if (_resultsAreStale || position != _lastPosition || radius != _lastRadius || layerMask != _lastLayerMask || alwaysUpdate)
        {
            UpdateOverlapSphere(position, radius, layerMask);
        }

        
        List<T> results = new List<T>();
        for (int i = 0; i < _lastCount; i++)
        {
            if (_overlapResults[i].TryGetComponent(out T component))
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
        Gizmos.DrawWireSphere(_lastPosition, _lastRadius);

        // Виділення знайдених об'єктів
        Gizmos.color = Color.yellow;
        foreach (var collider in _debugColliders)
        {
            if (collider != null)
            {
                Gizmos.DrawSphere(collider.transform.position, 0.3f);
            }
        }
    }
}
