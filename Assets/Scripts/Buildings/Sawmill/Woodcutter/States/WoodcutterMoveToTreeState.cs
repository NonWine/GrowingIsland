using Zenject;

public class WoodcutterMoveToTreeState : WoodcutterState
{
    [Inject] private NPCAnimator _animator;

    public override void Enter()
    {
        SetDestination();
    }

    public override void Tick()
    {
        if (Ctx.Agent.remainingDistance <= workSettings.ChopDistance)
        {
            ChangeState<WoodcutterChopState>();
        }
    }

    private void SetDestination()
    {
        Ctx.Agent.isStopped = false;
        _animator.SetMove();
        Ctx.Agent.SetDestination(woodCutterFacade.CurrentTree.transform.position);
    }

    public override void Exit()
    {
    }
}
