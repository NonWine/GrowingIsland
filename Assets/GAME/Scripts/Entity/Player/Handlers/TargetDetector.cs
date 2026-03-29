using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class TargetDetector
{
    private readonly Transform playerTransform;
    private readonly OverlapSphereHandler overlapHandler;
    private readonly float radius;
    private readonly LayerMask enemyMask;

    private readonly List<IDamageable> detected = new(10);
    private IDamageable lastDetectedTarget;

    public TargetDetector(Transform playerTransform, float radius, LayerMask enemyMask)
    {
        this.playerTransform = playerTransform;
        this.radius = radius;
        this.enemyMask = enemyMask;
        overlapHandler = new OverlapSphereHandler(20);
    }

    public IDamageable GetNearestTarget()
    {
        detected.Clear();
        var enemies = overlapHandler.GetFilteredObjects<IDamageable>(
            playerTransform.position,
            radius,
            enemyMask,
            e => e.isAlive,
            true);

        if (enemies.Count == 0)
        {
            lastDetectedTarget = null;
            return null;
        }

        Transform nearest = playerTransform.GetNearestTarget(enemies.ConvertAll(e => e.transform));
        lastDetectedTarget = nearest.GetComponent<IDamageable>();
        return lastDetectedTarget;
    }

    public bool IsTargetWithinRange(Vector3 targetPos)
    {
        Vector3 playerPos = playerTransform.position;
        playerPos.y = targetPos.y; // вирівняли по висоті
        return (targetPos - playerPos).sqrMagnitude <= radius * radius;
    }


    public void DrawGizmos()
    {

        // Радіус детекції
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerTransform.position, radius);
        
        if (playerTransform == null || lastDetectedTarget == null) return;
        // Активна ціль
        if (IsTargetWithinRange(lastDetectedTarget.transform.position) && lastDetectedTarget is Component c)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(c.transform.position, 0.3f);
            Gizmos.DrawLine(playerTransform.position, c.transform.position);
        }
    }
}

