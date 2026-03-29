using UnityEngine;

public class NPCAnimator : IEntityAnimateable
{
    private readonly Animator animator;
    private const string MOVING_KEY = "Speed";
    private const string STATE_KEY = "State";
    private const string STATE_LAYER_KEY = "StateBehavior";
    private readonly int attackStateIndex;

    public Animator Animator => animator;
    
    
    public NPCAnimator(Animator animator, int attackStateIndex)
    {
        this.animator = animator;
        this.attackStateIndex = attackStateIndex;
    }

    public void UpdateAnimator()
    {
        animator.SetInteger(STATE_KEY, 0);
        animator.SetFloat(MOVING_KEY, 0);
    }

    public void SetMove()
    {
        animator.SetLayerWeight(animator.GetLayerIndex(STATE_LAYER_KEY), 0);
        animator.SetFloat(MOVING_KEY, 1);
        animator.SetInteger(STATE_KEY, 0);
    }

    public void SetAttack()
    {
        animator.SetLayerWeight(animator.GetLayerIndex(STATE_LAYER_KEY), 0.8f);
        animator.SetInteger(STATE_KEY, attackStateIndex);
        animator.SetFloat(MOVING_KEY, 0);
    }

    public void SetIdle()
    {
        UpdateAnimator();
    }
}