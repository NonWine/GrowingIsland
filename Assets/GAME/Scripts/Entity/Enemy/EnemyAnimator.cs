using UnityEngine;

public class EnemyAnimator 
{
    protected Animator Animator;
    protected const  string _STATE_KEY = "state";
    protected const  string _STATE_LAYER_KEY = "StateBehavior";
    
    public EnemyAnimator(Animator animator)
    {
        Animator = animator;
    }
    
    public void Idle() =>         Animator.SetInteger(_STATE_KEY,0);
    
    public void Attack() =>         Animator.SetInteger(_STATE_KEY,2);
    
    public void Move() =>         Animator.SetInteger(_STATE_KEY,1);


    public void Die() =>         Animator.SetInteger(_STATE_KEY,4);
    
    public void GetDamage() =>         Animator.SetInteger(_STATE_KEY,3);

    
 
}