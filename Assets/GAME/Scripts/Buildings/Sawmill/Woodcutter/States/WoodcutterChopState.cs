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
        var worldHitDamageable = woodCutterFacade.CurrentTree.GetComponent<IDamageable>();
        if (worldHitDamageable == null)
            return;

        worldHitDamageable.GetDamage(workSettings.TreeDamage, view.VisualRoot.position);

        if (!woodCutterFacade.CurrentTree.IsAlive)
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
