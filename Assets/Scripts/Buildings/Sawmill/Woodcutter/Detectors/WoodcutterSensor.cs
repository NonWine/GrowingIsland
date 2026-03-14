using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Відповідає за виявлення ігрових об'єктів (дерев, ресурсів) у радіусі дії дроворуба.
/// </summary>
public class WoodcutterSensor : IWoodcutterSensor
{
    private readonly OverlapSphereHandler overlap;
    private readonly WoodcutterWorkSettings settings;
    private readonly WoodcutterView woodcutterView;

    [Inject]
    public WoodcutterSensor(OverlapSphereHandler overlap, WoodcutterWorkSettings settings, WoodcutterView woodcutterView)
    {
        this.overlap = overlap ?? throw new ArgumentNullException(nameof(overlap));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.woodcutterView = woodcutterView != null ? woodcutterView : throw new ArgumentNullException(nameof(woodcutterView));
    }

    #region Public API

    /// <summary>
    /// Знаходить найближче живе дерево для рубки.
    /// </summary>
    public bool TryFindNearest(out EnvironmentResource tree)
    {
        tree = FindNearest<EnvironmentResource>(
            settings.TreeSearchRadius, 
            settings.ResourceMask, 
            TreeFilter);
            
        return tree != null;
    }



    /// <summary>
    /// Знаходить найближчий випавший ресурс (деревину).
    /// </summary>
    public ResourcePartObj AcquireNearestDrop()
    {
        return FindNearest<ResourcePartObj>(
            settings.DropDetectionRadius, 
            settings.ResourcePartMask, 
            WoodDropFilter);
    }

    /// <summary>
    /// Повертає список усіх випавших ресурсів у вказаному радіусі.
    /// </summary>
    public List<ResourcePartObj> GetDropsInRadius(float radius)
    {
        return GetFiltered<ResourcePartObj>(
            radius, 
            settings.ResourcePartMask, 
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
        Vector3 origin = woodcutterView.transform.position;

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
        return overlap.GetFilteredObjects<T>(
            woodcutterView.transform.position,
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
