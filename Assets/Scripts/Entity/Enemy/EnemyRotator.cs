using UnityEngine;

public class EnemyRotator
{
    private readonly Transform _enemyTransform;
    private readonly Transform _playerTransform;
    private readonly Vector3 spawnPoint;
    private readonly EnemyStats enemyStats;

    public EnemyRotator(Transform enemyTransform, Transform playerTransform,Vector3 spawnPoint ,EnemyStats enemyStats)
    {
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        this.enemyStats = enemyStats;
        this.spawnPoint = spawnPoint;
    }

    public void RotateToPlayer()
    {
        if (_playerTransform == null) return;

        Vector3 direction = (_playerTransform.position - _enemyTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _enemyTransform.rotation = Quaternion.Slerp(_enemyTransform.rotation, targetRotation, Time.deltaTime * enemyStats.RotateSpeed);
    }

    public void RotateToSpawnPoint()
    {
        RotateToPoint(spawnPoint);
    }

    public void RotateToPoint(Vector3 point)
    {
        Vector3 enemyPos = _enemyTransform.position;
        Vector3 targetPos = point;

        targetPos.y = enemyPos.y;

        Vector3 direction = (targetPos - enemyPos).normalized;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _enemyTransform.rotation = Quaternion.Slerp(
            _enemyTransform.rotation,
            targetRotation,
            Time.deltaTime * enemyStats.RotateSpeed
        );
    }
}