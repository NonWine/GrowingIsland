using UnityEngine;

[System.Serializable]
public class WoodcutterChopState : WoodcutterState
{
    private readonly NPCAnimator _npcAnimator;
    private float _timer;

    public WoodcutterChopState(NPCAnimator npcAnimator)
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
        Debug.Log("Chop Tree damage: " + workSettings.TreeDamage );
        woodCutterFacade.CurrentTree.GetDamage(workSettings.TreeDamage);

        if (!woodCutterFacade.CurrentTree.isAlive)
        {
            woodCutterFacade.ClearTree();
            ChangeState<WoodcutterCollectState>();
        }
    }

    public override void Exit()
    {
        _npcAnimator.SetIdle();
    }
}
