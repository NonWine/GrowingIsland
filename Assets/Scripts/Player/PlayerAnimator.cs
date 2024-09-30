using UnityEngine;

public class PlayerAnimator : IEntityAnimateable
{
    private PlayerContainer _playerContainer;

    public PlayerAnimator(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
    }
    
    public void UpdateAnimator() 
    {
        if(_playerContainer.Direction != Vector3.zero)
            _playerContainer.Animator.SetBool("Moving", true);
        else
        {
            _playerContainer.Animator.SetBool("Moving", false);

        }
    } 
}