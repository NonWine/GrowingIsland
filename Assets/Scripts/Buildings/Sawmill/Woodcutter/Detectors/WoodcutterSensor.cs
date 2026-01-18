using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Відповідає за виявлення ігрових об'єктів (дерев, ресурсів) у радіусі дії дроворуба.
/// </summary>
public class WoodcutterSensor
{
    private readonly OverlapSphereHandler _overlap;
    private readonly WoodcutterWorkSettings _settings;
    private readonly Transform _transform;

    [Inject]
    public WoodcutterSensor(OverlapSphereHandler overlap, WoodcutterWorkSettings settings, Transform transform)
    {
        _overlap = overlap ?? throw new ArgumentNullException(nameof(overlap));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
    }

    #region Public API

    /// <summary>
    /// Знаходить найближче живе дерево для рубки.
    /// </summary>
    public bool TryFindNearestTree(out EnvironmentResource tree)
    {
        tree = FindNearest<EnvironmentResource>(
            _settings.TreeSearchRadius, 
            _settings.ResourceMask, 
            TreeFilter);
            
        return tree != null;
    }

    /// <summary>
    /// Знаходить найближчий випавший ресурс (деревину).
    /// </summary>
    public ResourcePartObj AcquireNearestDrop()
    {
        return FindNearest<ResourcePartObj>(
            _settings.DropDetectionRadius, 
            _settings.ResourcePartMask, 
            WoodDropFilter);
    }

    /// <summary>
    /// Повертає список усіх випавших ресурсів у вказаному радіусі.
    /// </summary>
    public List<ResourcePartObj> GetDropsInRadius(float radius)
    {
        return GetFiltered<ResourcePartObj>(
            radius, 
            _settings.ResourcePartMask, 
            WoodDropFilter);
    }

    #endregion

    #region Private Logic (Generic)

    private T FindNearest<T>(float radius, LayerMask mask, Func<T, bool> filter) where T : Component
    {
        var objects = GetFiltered<T>(radius, mask, filter);

        if (objects == null || objects.Count == 0)
            return null;

        T nearest = null;
        float minSqrDist = float.MaxValue;
        Vector3 origin = _transform.position;

        for (int i = 0; i < objects.Count; i++)
        {
            var obj = objects[i];
            if (obj == null) continue;
            
            float sqrDist = (obj.transform.position - origin).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearest = obj;
            }
        }

        return nearest;
    }

    private List<T> GetFiltered<T>(float radius, LayerMask mask, Func<T, bool> filter) where T : Component
    {
        return _overlap.GetFilteredObjects<T>(
            _transform.position,
            radius,
            mask,
            filter,
            true);
    }

    #endregion

    #region Static Filters (No GC Alloc for closures)

    private static bool TreeFilter(EnvironmentResource r) => 
        r != null && r.isAlive && r.ResourceType == eCollectable.Wood;

    private static bool WoodDropFilter(ResourcePartObj obj) => 
        obj != null && obj.TypeE == eCollectable.Wood && !obj.IsPicked;

    #endregion
}
