using UnityEngine;

public class WoodcutterChopState : WoodcutterState
{
    private readonly NPCAnimator _npcAnimator;
    private float _timer;

    public WoodcutterChopState(WoodcutterView context, WoodcutterStateMachine stateMachine, NPCAnimator npcAnimator) : base(context, stateMachine)
    {
        _npcAnimator = npcAnimator;
    }

    public override void Enter()
    {
        _timer = 0f;
        Ctx.Agent.isStopped = true;
        _npcAnimator.SetAttack();
    }

    public override void Tick()
    {
        _timer += Time.deltaTime;
        if (_timer < woodCutterFacade.ChopInterval)
            return;

        _timer = 0f;
        woodCutterFacade.CurrentTree.GetDamage(workSettings.TreeDamage);

        if (!woodCutterFacade.CurrentTree.isAlive)
        {
            woodCutterFacade.ClearTree();
            StateMachine.ChangeState<WoodcutterCollectState>();
        }
    }

    public override void Exit()
    {
        _npcAnimator.SetIdle();
    }
}
