using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PlayerAttackState : PlayerState
{
    private readonly PlayerAnimator _animator;
    private readonly PlayerRotating _rotator;
    private readonly PlayerAttackHandler _attackHandler;
    private readonly TargetDetector _detector;

    [ShowInInspector] private IDamageable _currentTarget;

    public PlayerAttackState(
        PlayerStateMachine stateMachine,
        PlayerContainer playerContainer,
        PlayerAnimator animator,
        PlayerRotating rotator,
        PlayerAttackHandler attackHandler,
        TargetDetector detector)
        : base(stateMachine, playerContainer)
    {
        _animator = animator;
        _rotator = rotator;
        _attackHandler = attackHandler;
        _detector = detector;
    }

    public override void Enter()
    {
        player.PlayerAnimatorEvent.OnFarming += TryAttack;
    }

    public override void LogicUpdate()
    {
        _currentTarget ??= _detector.GetNearestTarget();

        if (_currentTarget == null)
        {
            ResetTarget();
            return;
        }
        
        _animator.SetAnimataionLayerWeightBehaviour(1);
        _animator.Lumbering();
        _rotator.SetTargetRotate(_currentTarget.transform);

        if (!_currentTarget.isAlive ||
            !_detector.IsTargetWithinRange(_currentTarget.transform.position))
        {
            ResetTarget();
        }
    }

    private void TryAttack()
    {
        _attackHandler.TryAttack(_currentTarget, ResetTarget);
    }

    private void ResetTarget()
    {
        _animator.SetAnimataionLayerWeightBehaviour(0);
        _currentTarget = null;
        _rotator.UnLockTarget();
    }

    public override void Exit()
    {
        player.PlayerAnimatorEvent.OnFarming -= TryAttack;
        ResetTarget();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        _detector.DrawGizmos();
    }
}