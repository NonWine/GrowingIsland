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
        if(Input.GetKey(KeyCode.Space)) Debug.Log(_isTargeting);
       
        if(_isTargeting || _playerContainer.Direction == Vector3.zero)
           return;
        
        _playerContainer.Body.rotation = Quaternion.Slerp(_playerContainer.Body.rotation, Quaternion.LookRotation(_playerContainer.Direction, Vector3.up),
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