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
        if (view.Agent.remainingDistance <= workSettings.ChopDistance)
        {
            ChangeState<WoodcutterChopState>();
        }
    }

    private void SetDestination()
    {
        view.Agent.isStopped = false;
        _animator.SetMove();
        view.Agent.SetDestination(woodCutterFacade.CurrentTree.transform.position);
    }

    public override void Exit()
    {
    }
}
