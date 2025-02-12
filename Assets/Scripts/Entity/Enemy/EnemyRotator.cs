using UnityEngine;

public class EnemyRotator
{
    private readonly Transform _enemyTransform;
    private readonly Transform _playerTransform;
    private readonly float _rotationSpeed;

    public EnemyRotator(Transform enemyTransform, Transform playerTransform, float rotationSpeed)
    {
        _enemyTransform = enemyTransform;
        _playerTransform = playerTransform;
        _rotationSpeed = rotationSpeed;
    }

    public void RotateToPlayer()
    {
        if (_playerTransform == null) return;

        Vector3 direction = (_playerTransform.position - _enemyTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _enemyTransform.rotation = Quaternion.Slerp(_enemyTransform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
}