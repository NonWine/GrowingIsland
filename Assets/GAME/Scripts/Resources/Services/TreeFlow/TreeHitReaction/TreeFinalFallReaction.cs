using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TreeFinalFallReaction : TreeReactionBase, ITreeFinalFallReaction
{
    private readonly TreeFinalFallSettings settings;
    private Sequence fallSequence;
    private UniTaskCompletionSource currentPlayCompletion;
    private Quaternion baseLocalRotation;
    private Vector3 baseLocalPosition;
    private bool basePoseCaptured;

    public TreeFinalFallReaction(EnvironmentPropObjectView view, TreeFinalFallSettings settings) : base(view)
    {
        this.settings = settings;

    }

    public UniTask Play(Vector3 sourceWorldPosition)
    {
        CacheBasePose();
        KillSequence(ref fallSequence);
        leavesBurster.PlayFinalHitBursts(settings);

        var fallRoot = View.FallRoot;
        var currentRotation = fallRoot.localRotation;
        var awayDirection = GetAwayDirectionLocal(fallRoot, sourceWorldPosition);
        var fallAxis = Vector3.Cross(Vector3.up, awayDirection).normalized;
        var preFallAngle = settings.FallAngle * 0.1f * settings.FinalBendMultiplier;
        var preFallRotation = currentRotation * Quaternion.AngleAxis(preFallAngle, fallAxis);
        var fallRotation = currentRotation * Quaternion.AngleAxis(settings.FallAngle, fallAxis);

        fallSequence = DOTween.Sequence();
        fallSequence.Append(fallRoot.DOLocalRotateQuaternion(preFallRotation, settings.FinalRorationDuration).SetEase(settings.FinalRorationEase));
        fallSequence.AppendInterval(settings.MicroHoldDuration);
        fallSequence.Append(fallRoot.DOLocalRotateQuaternion(fallRotation, settings.FallDuration).SetEase(Ease.InQuad));

        PlayImpactPosition();
        PlayImpactRotation();

        var playCompletion = new UniTaskCompletionSource();
        currentPlayCompletion = playCompletion;
        fallSequence.OnComplete(() => CompletePlay(playCompletion));
        fallSequence.OnKill(() => CompletePlay(playCompletion));
        fallSequence.SetLink(View.gameObject);
        return playCompletion.Task;
    }

    private void PlayImpactRotation()
    {
        if (settings.ImpactRotationPunch > 0f)
        {
            fallSequence.Join( View.FallRoot.DOPunchRotation(
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
            fallSequence.Append( View.FallRoot.DOPunchPosition(
                Vector3.down * settings.ImpactPositionPunch,
                settings.LandImpactDuration,
                vibrato: 1,
                elasticity: 0f));
        }
    }

    public override void ResetToNeutral()
    {
        KillSequence(ref fallSequence);
        ResetCapturedPose(basePoseCaptured, View.FallRoot, baseLocalRotation, baseLocalPosition);
    }

    public override void Initialize()
    {
        CacheBasePose();
        ResetToNeutral();
    }

    protected override void CacheBasePose()
    {
        CapturePoseOnce(View.FallRoot, ref basePoseCaptured, ref baseLocalPosition, ref baseLocalRotation);
    }

    private void CompletePlay(UniTaskCompletionSource playCompletion)
    {
        if (currentPlayCompletion == playCompletion)
        {
            currentPlayCompletion = null;
        }

        playCompletion.TrySetResult();
    }
}
