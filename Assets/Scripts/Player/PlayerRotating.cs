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

    public void SetTargetRotate(Vector3 target)
    {
        _isTargeting = true;
        _playerContainer.Body.rotation = Quaternion.Slerp(_playerContainer.Body.rotation,
            Quaternion.LookRotation(target),
            _playerContainer.PlayerStats.RotateSpeed * Time.deltaTime);
    }

    public void UnLockTarget()
    {
        _isTargeting = true;
    }
}