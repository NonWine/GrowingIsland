using UnityEngine;

[System.Serializable]
public class WoodcutterChopState : WoodcutterState
{
    private readonly NPCAnimator _npcAnimator;
    private WoodcutterWorkSettings woodcutterWorkSettings;
    private float _timer;

    public WoodcutterChopState(NPCAnimator npcAnimator, WoodcutterWorkSettings woodcutterWorkSettings)
    {
        _npcAnimator = npcAnimator;
        this.woodcutterWorkSettings = woodcutterWorkSettings;
    }

    public override void Enter()
    {
        _npcAnimator.Animator.SetFloat("AttackSpeed", 1f / woodcutterWorkSettings.ChopInterval);
        view.AnimationEventsView.OnDamageTriger.AddListener(SetDamage);
        _timer = 0f;
        view.Agent.isStopped = true;
        _npcAnimator.SetAttack();
    }

    public override void Tick()
    {
        
    }


    private void SetDamage()
    {
        woodCutterFacade.CurrentTree.GetDamage(workSettings.TreeDamage);

        if (!woodCutterFacade.CurrentTree.isAlive)
        {
            woodCutterFacade.ClearTree();
            ChangeState<WoodcutterCollectState>();
        }
    }

    public override void Exit()
    {
        view.AnimationEventsView.OnDamageTriger.RemoveListener(SetDamage);
        _npcAnimator.SetIdle();
    }
}
