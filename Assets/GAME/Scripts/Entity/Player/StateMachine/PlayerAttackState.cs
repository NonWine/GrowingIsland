using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Time = UnityEngine.Time;

public class PlayerAttackState : PlayerState
{
    private readonly PlayerAnimator animator;
    private readonly PlayerRotating rotator;
    private readonly PlayerAttackHandler attackHandler;
    private readonly TargetDetector detector;
    private float timer;
    [ShowInInspector] private IDamageable currentTarget;

    public PlayerAttackState(
        PlayerStateMachine stateMachine,
        PlayerContainer playerContainer,
        PlayerAnimator animator,
        PlayerRotating rotator,
        PlayerAttackHandler attackHandler,
        TargetDetector detector)
        : base(stateMachine, playerContainer)
    {
        this.animator = animator;
        this.rotator = rotator;
        this.attackHandler = attackHandler;
        this.detector = detector;
    }

    public override void Enter()
    {
        animator.SetFarmingAnim(eCollectable.Wood); //will be change later
        player.PlayerAnimatorEvent.OnFarming += TryAttack;
    }

    public override void LogicUpdate()
    {
        currentTarget ??= detector.GetNearestTarget();

        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }

        //tiemr for smooth rotation to target
        timer += Time.deltaTime;
        if (timer >= 0.3f)
        {
            animator.SetAnimataionLayerWeightBehaviour(1);
            animator.Lumbering();
            rotator.SetTargetRotate(currentTarget.transform);
        }

        if (!currentTarget.isAlive ||
            !detector.IsTargetWithinRange(currentTarget.transform.position))
        { 
            ResetTarget();
        }
    }

    private void TryAttack()
    {
        attackHandler.TryAttack(currentTarget, ResetTarget);
    }

    private void ResetTarget()
    {
        animator.SetAnimataionLayerWeightBehaviour(0);
        currentTarget = null;
        rotator.UnLockTarget();
        timer = 0f;
    }

    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnFarming -= TryAttack;
        ResetTarget();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        detector.DrawGizmos();
    }
}

