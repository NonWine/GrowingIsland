using System;
using UnityEngine;

public class PlayerMoving : IMoveable
{
    private PlayerContainer _playerContainer;

    public PlayerMoving(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
    }
    
    public void Move()
    {
        _playerContainer.Direction = new Vector3(_playerContainer.Joystick.Horizontal, 0, _playerContainer.Joystick.Vertical).normalized;
        _playerContainer.Agent.Move(   _playerContainer.Direction * (_playerContainer.PlayerStats.MoveSpeed * Time.deltaTime));
    }
}