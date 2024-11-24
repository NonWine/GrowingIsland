using System;
using UnityEngine;

public class PlayerAnimator : IEntityAnimateable, IDisposable
{
    private const  string _MOVING_KEY = "Speed";
    private const  string _STATE_KEY = "State";
    private const  string _STATE_LAYER_KEY = "StateBehavior";
    

    private PlayerContainer _playerContainer;

    public PlayerAnimator(PlayerContainer playerContainer)
    {
        _playerContainer = playerContainer;
        playerContainer.PlayerTrigger.CurrentResourceTrigger += SetFarmingAnim;
        //  playerContainer.Animator.setla
    }
    
    public void UpdateAnimator() 
    {
        if (_playerContainer.Direction != Vector3.zero)
        {
            _playerContainer.Animator.SetFloat(_MOVING_KEY, 1f);

        }
        else
        {
            _playerContainer.Animator.SetFloat(_MOVING_KEY, -1f);

        }
    }

    public void SetStateBehaviour(int state)
    {
        if (state == 0)
        {
            _playerContainer.Animator.SetInteger(_STATE_KEY,0);
            _playerContainer.Animator.SetLayerWeight(1,0);
        }
        else
            _playerContainer.Animator.SetLayerWeight(1,1);
    }
    
    public void Lumbering() =>         _playerContainer.Animator.SetInteger(_STATE_KEY,1);
    
    public void MineAttack() =>         _playerContainer.Animator.SetInteger(_STATE_KEY,2);

    public void Digging() =>         _playerContainer.Animator.SetInteger(_STATE_KEY,3);

    private void SetFarmingAnim(eCollectable wECollectable)
    {
        if(wECollectable == eCollectable.Wood)
            Lumbering();
        else if(wECollectable == eCollectable.Stone)
            MineAttack();
        else if(wECollectable == eCollectable.grass)
            Digging();
    }
    
    public void Dispose()
    {
        _playerContainer.PlayerTrigger.CurrentResourceTrigger -= SetFarmingAnim;
        Debug.Log(typeof(PlayerAnimator) + " Destroy ");
    }
}