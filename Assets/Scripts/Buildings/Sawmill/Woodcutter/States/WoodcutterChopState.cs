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
        _npcAnimator.Animator.SetFloat("AttackSpeed", 1f / woodCutterFacade.ChopInterval);
        Ctx.AnimationEventsView.OnDamageTriger.AddListener(SetDamage);
        _timer = 0f;
        Ctx.Agent.isStopped = true;
        _npcAnimator.SetAttack();
    }

    public override void Tick()
    {
        
    }


    private void SetDamage()
    {
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
        Ctx.AnimationEventsView.OnDamageTriger.RemoveListener(SetDamage);
        _npcAnimator.SetIdle();
    }
}
