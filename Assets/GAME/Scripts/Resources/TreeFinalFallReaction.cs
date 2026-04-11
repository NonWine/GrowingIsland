using DG.Tweening;
using UnityEngine;

public class TreeFinalFallReaction : TreeReactionBase, ITreeFinalFallReaction
{
    private readonly EnvironmentPropObjectView view;
    private readonly TreeFinalFallSettings settings;
    private Sequence fallSequence;
    private Quaternion baseLocalRotation;
    private Vector3 baseLocalPosition;
    private bool basePoseCaptured;

    public TreeFinalFallReaction(EnvironmentPropObjectView view, TreeFinalFallSettings settings) : base(view)
    {
        this.view = view;
        this.settings = settings;

    }

    public void Play(Vector3 sourceWorldPosition)
    {
        CacheBasePose();
        KillSequence(ref fallSequence);
        ResetToNeutral();
        leavesBurster.PlayFinalHitBursts(settings);

        var fallRoot = view.FallRoot;
        var awayDirection = GetAwayDirectionLocal(fallRoot, sourceWorldPosition);
        var fallAxis = Vector3.Cross(Vector3.up, awayDirection).normalized;
        var preFallAngle = settings.FallAngle * 0.12f * settings.FinalBendMultiplier;
        var preFallRotation = baseLocalRotation * Quaternion.AngleAxis(preFallAngle, fallAxis);
        var fallRotation = baseLocalRotation * Quaternion.AngleAxis(settings.FallAngle, fallAxis);

        fallSequence = DOTween.Sequence();
        fallSequence.Append(fallRoot.DOLocalRotateQuaternion(preFallRotation, settings.MicroHoldDuration)
            .SetEase(Ease.OutQuad));
        fallSequence.AppendInterval(settings.MicroHoldDuration);
        fallSequence.Append(fallRoot.DOLocalRotateQuaternion(fallRotation, settings.FallDuration)
            .SetEase(Ease.InQuad));
        fallSequence.AppendCallback(() => leavesBurster.PlayImpactBursts(settings));

        PlayImpactPosition();
        PlayImpactRotation();

        fallSequence.SetLink(view.gameObject);
    }

    private void PlayImpactRotation()
    {
        if (settings.ImpactRotationPunch > 0f)
        {
            fallSequence.Join( view.FallRoot.DOPunchRotation(
                new Vector3(0f, 0f, settings.ImpactRotationPunch),
                settings.LandImpactDuration,
                vibrato: 1,
                elasticity: 0f));
        }
    }

    private void PlayImpactPosition()
    {
        if (settings.ImpactPositionPunch > 0f)
        {
            fallSequence.Append( view.FallRoot.DOPunchPosition(
                Vector3.down * settings.ImpactPositionPunch,
                settings.LandImpactDuration,
                vibrato: 1,
                elasticity: 0f));
        }
    }

    public override void ResetToNeutral()
    {
        KillSequence(ref fallSequence);
        ResetCapturedPose(basePoseCaptured, view.FallRoot, baseLocalRotation, baseLocalPosition);
    }

    public override void Initialize()
    {
        CacheBasePose();
        ResetToNeutral();
    }

    protected override void CacheBasePose()
    {
        CapturePoseOnce(view.FallRoot, ref basePoseCaptured, ref baseLocalPosition, ref baseLocalRotation);
    }
}