using UnityEngine;

public class PlayerRotating : IRotateable
{
    private PlayerContainer _playerContainer;
    private bool _isTargeting;
    
    public PlayerRotating(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
    }

    public void  Rotate()
    {
       if(_isTargeting)
           return;
        
            if (_playerContainer.Direction != Vector3.zero)
                _playerContainer.Body.rotation = Quaternion.Slerp(_playerContainer.Body.rotation,
                    Quaternion.LookRotation(_playerContainer.Direction),
                    _playerContainer.PlayerStats.RotateSpeed * Time.deltaTime);

       

    }

    public void SetTargetRotate(Transform target)
    {
        _isTargeting = true;
        Vector3 direction = (target.position - _playerContainer.Body.position).normalized; // Отримуємо напрямок
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up); // Обчислюємо правильний поворот

        _playerContainer.Body.rotation = Quaternion.Slerp(
            _playerContainer.Body.rotation,
            targetRotation,
            _playerContainer.PlayerStats.RotateSpeed * Time.deltaTime
        );
    }


    public void UnLockTarget()
    {
        _isTargeting = false;
    }
}