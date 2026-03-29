using System;
using UnityEngine;

public class PlayerMoving : IMoveable
{
    private PlayerContainer playerContainer;

    public PlayerMoving(PlayerContainer playerContainer)
    {
        this.playerContainer = playerContainer;
    }
    
    public void Move()
    {
        
        
        playerContainer.Direction = new Vector3(playerContainer.Joystick.Horizontal, 0, playerContainer.Joystick.Vertical).normalized;
        
     //   playerContainer.Agent.Move(   playerContainer.Direction * (playerContainer.PlayerStats.MoveSpeed * Time.deltaTime));
    }
}

