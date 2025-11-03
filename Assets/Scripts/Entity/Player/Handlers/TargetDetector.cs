using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class TargetDetector
{
    private readonly Transform _playerTransform;
    private readonly OverlapSphereHandler _overlapHandler;
    private readonly float _radius;
    private readonly LayerMask _enemyMask;

    private readonly List<IDamageable> _detected = new(10);
    private IDamageable _lastDetectedTarget;

    public TargetDetector(Transform playerTransform, float radius, LayerMask enemyMask)
    {
        _playerTransform = playerTransform;
        _radius = radius;
        _enemyMask = enemyMask;
        _overlapHandler = new OverlapSphereHandler(20);
    }

    public IDamageable GetNearestTarget()
    {
        _detected.Clear();
        var enemies = _overlapHandler.GetFilteredObjects<IDamageable>(
            _playerTransform.position,
            _radius,
            _enemyMask,
            e => e.isAlive,
            true);

        if (enemies.Count == 0)
        {
            _lastDetectedTarget = null;
            return null;
        }

        Transform nearest = _playerTransform.GetNearestTarget(enemies.ConvertAll(e => e.transform));
        _lastDetectedTarget = nearest.GetComponent<IDamageable>();
        return _lastDetectedTarget;
    }

    public bool IsTargetWithinRange(Vector3 targetPos)
    {
        Vector3 playerPos = _playerTransform.position;
        playerPos.y = targetPos.y; // вирівняли по висоті
        return (targetPos - playerPos).sqrMagnitude <= _radius * _radius;
    }


    public void DrawGizmos()
    {

        // Радіус детекції
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_playerTransform.position, _radius);
        
        if (_playerTransform == null || _lastDetectedTarget == null) return;
        // Активна ціль
        if (IsTargetWithinRange(_lastDetectedTarget.transform.position) && _lastDetectedTarget is Component c)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(c.transform.position, 0.3f);
            Gizmos.DrawLine(_playerTransform.position, c.transform.position);
        }
    }
}