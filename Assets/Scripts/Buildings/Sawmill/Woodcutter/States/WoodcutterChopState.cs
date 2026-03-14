using UnityEngine;

[System.Serializable]
public class WoodcutterChopState : WoodcutterState
{
    private readonly NPCAnimator npcAnimator;
    private WoodcutterWorkSettings woodcutterWorkSettings;
    private float timer;

    public WoodcutterChopState(NPCAnimator npcAnimator, WoodcutterWorkSettings woodcutterWorkSettings)
    {
        this.npcAnimator = npcAnimator;
        this.woodcutterWorkSettings = woodcutterWorkSettings;
    }

    public override void Enter()
    {
        npcAnimator.Animator.SetFloat("AttackSpeed", 1f / woodcutterWorkSettings.ChopInterval);
        view.AnimationEventsView.OnDamageTriger.AddListener(SetDamage);
        timer = 0f;
        view.Agent.isStopped = true;
        npcAnimator.SetAttack();
    }

    public override void Tick()
    {
        
    }


    private void SetDamage()
    {
        if (woodCutterFacade.CurrentTree is IWorldHitDamageable worldHitDamageable)
        {
            worldHitDamageable.GetDamage(workSettings.TreeDamage, view.VisualRoot.position);
        }
        else
        {
            woodCutterFacade.CurrentTree.GetDamage(workSettings.TreeDamage);
        }

        if (!woodCutterFacade.CurrentTree.isAlive)
        {
            woodCutterFacade.ClearTree();
            ChangeState<WoodcutterCollectState>();
        }
    }

    public override void Exit()
    {
        view.AnimationEventsView.OnDamageTriger.RemoveListener(SetDamage);
        npcAnimator.SetIdle();
    }
}
