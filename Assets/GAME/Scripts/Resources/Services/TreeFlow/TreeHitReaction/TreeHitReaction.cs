using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TreeHitReaction : TreeReactionBase, ITreeHitReaction
{
    private readonly TreeHitAnimationSettings settings;

    private bool basePoseCaptured;
    private Quaternion mainBaseRotation;
    private Quaternion currentStartRotation;
    private Quaternion mainBendRotation;
    private Quaternion mainOvershootRotation;
    private UniTaskCompletionSource currentPlayCompletion;

    private Sequence sequence;

    public TreeHitReaction(EnvironmentPropObjectView view, TreeHitAnimationSettings settings) : base(view)
    {
        this.settings = settings;
    }

    public UniTask Play(Vector3 sourceWorldPosition)
    {
        KillSequence(ref sequence);
        currentStartRotation = View.ReactionRoot.localRotation;

        ParticlePool.Instance.PlayAxeHitFx(View.transform.position);
        leavesBurster.PlayLeavesBursts(settings);

        var awayLocal = GetAwayDirectionLocal(View.ReactionRoot, sourceWorldPosition);
        var bendAxis = Vector3.Cross(Vector3.up, awayLocal).normalized;

        var angleScale = 1f + Random.Range(-settings.AngleVariance, settings.AngleVariance);
        var durationMultiplier = 1f + Random.Range(-settings.DurationVariance, settings.DurationVariance);

        var bendAngle = settings.MainBendAngle * angleScale;
        var overshootAngle = settings.OvershootAngle * angleScale;
        var hitDuration = settings.BaseHitDuration * durationMultiplier;
        var mainOvershootDuration = settings.OvershootDuration * durationMultiplier;
        var mainSettleDuration = settings.SettleDuration * durationMultiplier;

        mainBendRotation = currentStartRotation * Quaternion.AngleAxis(bendAngle, bendAxis);
        mainOvershootRotation = currentStartRotation * Quaternion.AngleAxis(-overshootAngle, bendAxis);
   

        sequence = DOTween.Sequence();
        sequence.Append(View.ReactionRoot.DOLocalRotateQuaternion(mainBendRotation, hitDuration).SetEase(Ease.OutQuad));
        sequence.Append(View.ReactionRoot.DOLocalRotateQuaternion(mainOvershootRotation, mainOvershootDuration).SetEase(Ease.InOutQuad));
        sequence.Append(View.ReactionRoot.DOLocalRotateQuaternion(mainBaseRotation, mainSettleDuration).SetEase(Ease.OutCubic));
        var playCompletion = new UniTaskCompletionSource();
        currentPlayCompletion = playCompletion;
        sequence.OnComplete(() => CompletePlayAndRelease(playCompletion));
        sequence.OnKill(() => CompletePlay(playCompletion));
        sequence.SetLink(View.gameObject);
        
        return playCompletion.Task;

    }

    public override void ResetToNeutral()
    {
        KillSequence(ref sequence);
        ResetCapturedPose(basePoseCaptured, View.ReactionRoot, mainBaseRotation);
    }

    public override void Initialize()
    {
        CacheBasePose();
        ResetToNeutral();
    }

    protected override void CacheBasePose()
    {
        if (basePoseCaptured)
        {
            return;
        }

        mainBaseRotation = View.ReactionRoot.localRotation;
        basePoseCaptured = true;
    }
    
    private void CompletePlay(UniTaskCompletionSource playCompletion)
    {
        if (currentPlayCompletion == playCompletion)
        {
            currentPlayCompletion = null;
        }

        playCompletion.TrySetResult();
    }

    private void CompletePlayAndRelease(UniTaskCompletionSource playCompletion)
    {
        sequence = null;
        CompletePlay(playCompletion);
    }
}
