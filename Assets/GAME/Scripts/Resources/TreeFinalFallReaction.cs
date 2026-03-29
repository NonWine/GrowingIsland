using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

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
        CacheBasePose();
        ResetToNeutral();
    }

    public void Play(Vector3 sourceWorldPosition)
    {
        CacheBasePose();
        KillSequence(ref fallSequence);
        ResetToNeutral();
        PlayFinalHitBursts();

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
        fallSequence.AppendCallback(PlayImpactBursts);

        if (settings.ImpactPositionPunch > 0f)
        {
            fallSequence.Append(fallRoot.DOPunchPosition(
                Vector3.down * settings.ImpactPositionPunch,
                settings.LandImpactDuration,
                vibrato: 1,
                elasticity: 0f));
        }

        if (settings.ImpactRotationPunch > 0f)
        {
            fallSequence.Join(fallRoot.DOPunchRotation(
                new Vector3(0f, 0f, settings.ImpactRotationPunch),
                settings.LandImpactDuration,
                vibrato: 1,
                elasticity: 0f));
        }

        fallSequence.SetLink(view.gameObject);
    }

    public override void ResetToNeutral()
    {
        KillSequence(ref fallSequence);
        ResetCapturedPose(basePoseCaptured, view.FallRoot, baseLocalRotation, baseLocalPosition);
    }

    protected override void CacheBasePose()
    {
        CapturePoseOnce(view.FallRoot, ref basePoseCaptured, ref baseLocalPosition, ref baseLocalRotation);
    }

    private void PlayFinalHitBursts()
    {
        var particlePool = ParticlePool.Instance;
        var trunkOrigin = view.ReactionRoot.position;
        for (int i = 0; i < settings.FinalTrunkFxBursts; i++)
        {
            particlePool.PlayAxeHitFx(trunkOrigin + RandomHorizontalOffset(settings.FinalTrunkFxJitter));
        }

        var leavesPoints = view.LeavesPoints;
        if (leavesPoints.Length == 0)
        {
            return;
        }

        int maxBursts = Mathf.Max(settings.FinalLeafBurstsMin, settings.FinalLeafBurstsMax);
        int burstsCount = Random.Range(settings.FinalLeafBurstsMin, maxBursts + 1);
        burstsCount = Mathf.Max(
            settings.FinalLeafBurstsMin,
            Mathf.RoundToInt(burstsCount * settings.FinalLeavesMultiplier));

        for (int i = 0; i < burstsCount; i++)
        {
            var point = leavesPoints[Random.Range(0, leavesPoints.Length)];
            particlePool.PlayFallenLeaves(point.position + Random.insideUnitSphere * settings.FinalLeafPositionJitter);
        }
    }

    private void PlayImpactBursts()
    {
        var particlePool = ParticlePool.Instance;
        var impactOrigin = view.GroundImpactPoint.position;
        for (int i = 0; i < settings.ImpactDustBursts; i++)
        {
            particlePool.PlayPoof(impactOrigin + RandomHorizontalOffset(settings.ImpactDustJitter));
        }
    }

    private static Vector3 RandomHorizontalOffset(float radius)
    {
        if (radius <= 0f)
        {
            return Vector3.zero;
        }

        Vector2 offset2D = Random.insideUnitCircle * radius;
        return new Vector3(offset2D.x, 0f, offset2D.y);
    }
}
