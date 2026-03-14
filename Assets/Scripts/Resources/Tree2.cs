using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class Tree2 : EnvironmentResource
{
    [FormerlySerializedAs("_animSettings")]
    [SerializeField] private TreeHitAnimationSettings animSettings = new();
    [SerializeField] private TreeFinalFallSettings finalFallSettings = new();
    [SerializeField] private TreeView view;

    [Inject] private ITreeHitReactionFactory hitReactionFactory;

    private ITreeHitReaction hitReaction;
    private ITreeFinalFallReaction finalFallReaction;
    private TreeStumpPresenter stumpPresenter;
    private Tween finalFallImpactTween;
    private Tween finalFallCompleteTween;

    private void Start()
    {
        hitReaction = hitReactionFactory.Create(view, animSettings);
        finalFallReaction = new TreeFinalFallReaction(view, finalFallSettings);
        stumpPresenter = new TreeStumpPresenter(view);
    }

    private void OnDisable()
    {
        KillFinalFallImpactTween();
        KillFinalFallCompleteTween();
        hitReaction.ResetToNeutral();
        finalFallReaction.ResetToNeutral();
        stumpPresenter.Hide();
    }

    private void OnDestroy()
    {
        KillFinalFallImpactTween();
        KillFinalFallCompleteTween();
        stumpPresenter.Dispose();
    }

    protected override void AnimTrigDamage(Vector3 sourceWorldPosition, bool isFinalHit)
    {
        if (isFinalHit)
        {
            finalFallReaction.Play(sourceWorldPosition);
            ScheduleFinalFallImpact(HandleFinalFallImpact);
            return;
        }

        hitReaction.PlayHit(sourceWorldPosition, isFinalHit);
    }

    protected override void HandleFinalHit(Vector3 sourceWorldPosition)
    {
        AnimTrigDamage(sourceWorldPosition, true);
    }

    public override void GetDamage(float damage)
    {
        bool wasAlive = isAlive;
        base.GetDamage(damage);
        if (wasAlive && isAlive)
        {
            ParticlePool.Instance.PlayAxeHitFx(transform.position);
        }
    }

    public override void GetDamage(float damage, Vector3 sourceWorldPosition)
    {
        bool wasAlive = isAlive;
        base.GetDamage(damage, sourceWorldPosition);
        if (wasAlive && isAlive)
        {
            ParticlePool.Instance.PlayAxeHitFx(transform.position);
        }
    }

    private void HandleFinalFallComplete()
    {
        HideResourceVisuals();
        stumpPresenter.Show();
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        BeginRespawn();
    }

    private void HandleFinalFallImpact()
    {
        SpawnResourceDrops();
        ScheduleFinalFallComplete(HandleFinalFallComplete);
    }

    protected override void OnRespawnCompleted()
    {
        stumpPresenter.Hide();
    }

    public void DebugPreviewHit()
    {
        hitReaction.PlayHit(GetPreviewHitSource());
    }

    public void DebugPreviewFinalFall()
    {
        finalFallReaction.Play(GetPreviewHitSource());
    }

    public void DebugPreviewFullFinalHit()
    {
        finalFallReaction.Play(GetPreviewHitSource());
        ScheduleFinalFallImpact(HandlePreviewFinalFallImpact);
    }

    public void DebugResetPreview()
    {
        finalFallReaction.ResetToNeutral();
        hitReaction.ResetToNeutral();
        ShowResourceVisuals();
        stumpPresenter.Hide();
        KillFinalFallImpactTween();
        KillFinalFallCompleteTween();
    }

    private void HandlePreviewFinalFallComplete()
    {
        HideResourceVisuals();
        stumpPresenter.Show();
    }

    private void HandlePreviewFinalFallImpact()
    {
        SpawnResourceDrops();
        ScheduleFinalFallComplete(HandlePreviewFinalFallComplete);
    }

    private void ScheduleFinalFallComplete(TweenCallback onComplete)
    {
        KillFinalFallCompleteTween();
        finalFallCompleteTween = DOVirtual.DelayedCall(finalFallSettings.LandImpactDuration, onComplete)
            .SetLink(gameObject);
    }

    private void ScheduleFinalFallImpact(TweenCallback onImpact)
    {
        KillFinalFallImpactTween();
        finalFallImpactTween = DOVirtual.DelayedCall(GetFinalFallImpactDelay(), onImpact)
            .SetLink(gameObject);
    }

    private void KillFinalFallCompleteTween()
    {
        if (finalFallCompleteTween != null && finalFallCompleteTween.IsActive())
        {
            finalFallCompleteTween.Kill(false);
        }
    }

    private void KillFinalFallImpactTween()
    {
        if (finalFallImpactTween != null && finalFallImpactTween.IsActive())
        {
            finalFallImpactTween.Kill(false);
        }
    }

    private float GetFinalFallImpactDelay()
    {
        return finalFallSettings.MicroHoldDuration + finalFallSettings.MicroHoldDuration + finalFallSettings.FallDuration;
    }

    private Vector3 GetPreviewHitSource()
    {
        Vector3 previewSource = transform.position - transform.forward * 1.5f;
        previewSource.y = transform.position.y;
        return previewSource;
    }
}
