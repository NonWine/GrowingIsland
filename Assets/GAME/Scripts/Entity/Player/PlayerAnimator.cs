using System;
using UnityEngine;

public class PlayerAnimator : IEntityAnimateable
{
    private const  string _MOVING_KEY = "Speed";
    private const  string _STATE_KEY = "State";
    private const  string _STATE_LAYER_KEY = "StateBehavior";

    public PlayerAnimator(PlayerContainer playerContainer)
    {
        this.playerContainer = playerContainer;
    }
        
    
    private PlayerContainer playerContainer;


    
    public void UpdateAnimator() 
    {
        // ThirdPersonController drives the locomotion parameter on this animator.
        // This class only controls state-based actions like mining/lumbering/attack.
    }

    public void SetAnimataionLayerWeightBehaviour(int state)
    {
        if (state == 0)
        {
            playerContainer.Animator.SetInteger(_STATE_KEY,0);
            playerContainer.Animator.SetLayerWeight(1,0);
        }
        else
            playerContainer.Animator.SetLayerWeight(1,1);
    }
    
    public void Lumbering() =>         playerContainer.Animator.SetInteger(_STATE_KEY,1);
    
    public void MineAttack() =>         playerContainer.Animator.SetInteger(_STATE_KEY,2);

    public void Digging() =>         playerContainer.Animator.SetInteger(_STATE_KEY,3);
    
    public void Attack() =>playerContainer.Animator.SetInteger(_STATE_KEY,2);

    public void SetFarmingAnim(eCollectable wECollectable)
    {
        if(wECollectable == eCollectable.Wood)
            Lumbering();
        else if(wECollectable == eCollectable.Stone)
            MineAttack();
        else if(wECollectable == eCollectable.grass)
            Digging();
    }
    
}

