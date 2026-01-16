using System;
using System.Collections.Generic;
using UnityEngine;

public class WoodcutterResourceDetector
{
    private readonly WoodcutterContext _context;

    public WoodcutterResourceDetector(WoodcutterContext context)
    {
        _context = context;
    }

    public ResourcePartObj AcquireNearestDrop()
    {
        var drops = GetDropsWithFallback(_context.WorkSettings.DropDetectionRadius);
        if (drops.Count == 0)
        {
            return null;
        }

        Transform nearest = null;
        ResourcePartObj nearestDrop = null;
        var origin = _context.Transform.position;

        for (int i = 0; i < drops.Count; i++)
        {
            var drop = drops[i];
            if (drop == null)
                continue;

            var tr = drop.transform;
            var distSqr = (tr.position - origin).sqrMagnitude;
            if (nearest == null || distSqr < (nearest.position - origin).sqrMagnitude)
            {
                nearest = tr;
                nearestDrop = drop;
            }
        }

        return nearestDrop;
    }

    public List<ResourcePartObj> GetDropsWithFallback(float radius)
    {

        var primary = _context.OverlapSphereHandler.GetFilteredObjects(
            _context.Transform.position,
            radius,
            0,
            GetFilter(),
            true);
        
        return primary;
    }

    private Func<ResourcePartObj, bool> GetFilter() => obj => obj != null && obj.TypeE == eCollectable.Wood && !obj.IsPicked;
}
