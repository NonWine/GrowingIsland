using UnityEngine;

public class PlayerRotating : IRotateable
{
    private PlayerContainer _playerContainer;

    public PlayerRotating(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
    }

    public void  Rotate()
    {
        {
            if (_playerContainer.Direction != Vector3.zero)
                _playerContainer.Body.rotation = Quaternion.Slerp(_playerContainer.Body.rotation,
                    Quaternion.LookRotation(_playerContainer.Direction),
                    _playerContainer.PlayerStats.RotateSpeed * Time.deltaTime);

        }

    }
}