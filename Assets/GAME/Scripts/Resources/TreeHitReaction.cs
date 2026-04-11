using DG.Tweening;
using UnityEngine;

public class TreeHitReaction : TreeReactionBase, ITreeHitReaction
{
    private readonly EnvironmentPropObjectView view;
    private readonly TreeHitAnimationSettings settings;
    private readonly TreeLeavesBurster  leavesBurster;

    private Quaternion mainBaseRotation;
    private Quaternion mainBendRotation;
    private Quaternion mainOvershootRotation;

    private Sequence sequence;

    public TreeHitReaction(EnvironmentPropObjectView view, TreeHitAnimationSettings settings) : base(view)
    {
        this.view = view;
        this.settings = settings;
    }

    public void PlayHit(Vector3 sourceWorldPosition)
    {
        ParticlePool.Instance.PlayAxeHitFx(view.transform.position);

        KillSequence(ref sequence);
        leavesBurster.PlayLeavesBursts(settings);

        var awayLocal = GetAwayDirectionLocal(view.ReactionRoot, sourceWorldPosition);
        var bendAxis = Vector3.Cross(Vector3.up, awayLocal).normalized;

        var angleScale = 1f + Random.Range(-settings.AngleVariance, settings.AngleVariance);
        var durationScale = 1f + Random.Range(-settings.DurationVariance, settings.DurationVariance);

        var bendAngle = settings.MainBendAngle * angleScale;
        var overshootAngle = settings.OvershootAngle * angleScale;

        var mainBendDuration = settings.HitBendDuration * durationScale;
        var mainOvershootDuration = settings.OvershootDuration * durationScale;
        var mainSettleDuration = settings.SettleDuration * durationScale;

        mainBendRotation = mainBaseRotation * Quaternion.AngleAxis(bendAngle, bendAxis);
        mainOvershootRotation = mainBaseRotation * Quaternion.AngleAxis(-overshootAngle, bendAxis);

        sequence = DOTween.Sequence();
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainBendRotation, mainBendDuration).SetEase(Ease.OutQuad));
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainOvershootRotation, mainOvershootDuration).SetEase(Ease.InOutQuad));
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainBaseRotation, mainSettleDuration).SetEase(Ease.OutCubic));

        sequence.OnComplete(ResetToNeutral);
        sequence.SetLink(view.gameObject);
    }

    public override void ResetToNeutral()
    {
        KillSequence(ref sequence);
        ResetPose(view.ReactionRoot, mainBaseRotation);
    }

    public override void Initialize()
    {
        CacheBasePose();
        ResetToNeutral();
    }

    protected override void CacheBasePose()
    {
        mainBaseRotation = view.ReactionRoot.localRotation;
    }
}