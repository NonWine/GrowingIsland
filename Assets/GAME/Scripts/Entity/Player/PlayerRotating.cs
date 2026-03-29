using UnityEngine;

public class PlayerRotating : IRotateable
{
    private PlayerContainer playerContainer;
    private bool isTargeting;
    
    public PlayerRotating(PlayerContainer playerContainer)
    {
        this.playerContainer = playerContainer;
    }

    public void  Rotate()
    {
        if(Input.GetKey(KeyCode.Space)) Debug.Log(isTargeting);
       
        if(isTargeting || playerContainer.Direction == Vector3.zero)
           return;
        
        playerContainer.Body.rotation = Quaternion.Slerp(playerContainer.Body.rotation, Quaternion.LookRotation(playerContainer.Direction, Vector3.up),
                    playerContainer.PlayerStats.RotateSpeed * Time.deltaTime);

       

    }

    public void SetTargetRotate(Transform target)
    {
        isTargeting = true;
        Vector3 direction = (target.position - playerContainer.Body.position).normalized; // Отримуємо напрямок
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up); // Обчислюємо правильний поворот

        playerContainer.Body.rotation = Quaternion.Slerp(
            playerContainer.Body.rotation,
            targetRotation,
            playerContainer.PlayerStats.RotateSpeed * Time.deltaTime
        );
    }


    public void UnLockTarget()
    {
        isTargeting = false;
    }
}

