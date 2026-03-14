using UnityEngine;
using DG.Tweening;

public class TreeHitReaction : ITreeHitReaction
{
    private readonly TreeView view;
    private readonly TreeHitAnimationSettings settings;
    private readonly bool hasSecondaryLag;

    private Quaternion mainBaseRotation;
    private Quaternion crownBaseRotation;

    private Quaternion mainStartRotation;
    private Quaternion mainBendRotation;
    private Quaternion mainOvershootRotation;

    private Sequence sequence;

    public TreeHitReaction(TreeView view, TreeHitAnimationSettings settings)
    {
        this.view = view;
        this.settings = settings;
        hasSecondaryLag = view.CrownRoot != view.ReactionRoot;

        CacheBasePose();
        ResetToNeutral();
    }

    public void PlayHit(Vector3 sourceWorldPosition, bool isFinalHit = false)
    {
        KillActiveSequence();
        PlayLeavesBursts(isFinalHit);

        var currentMainRotation = view.ReactionRoot.localRotation;
        var currentCrownRotation = view.CrownRoot.localRotation;

        var awayLocal = GetAwayDirectionLocal(sourceWorldPosition);
        var bendAxis = Vector3.Cross(Vector3.up, awayLocal).normalized;

        var finalMultiplier = isFinalHit ? settings.FinalHitMultiplier : 1f;
        var angleScale = 1f + Random.Range(-settings.AngleVariance, settings.AngleVariance);
        var durationScale = 1f + Random.Range(-settings.DurationVariance, settings.DurationVariance);

        var bendAngle = settings.MainBendAngle * finalMultiplier * angleScale;
        var overshootAngle = settings.OvershootAngle * finalMultiplier * angleScale;

        var mainBendDuration = settings.HitBendDuration * durationScale;
        var mainOvershootDuration = settings.OvershootDuration * durationScale;
        var mainSettleDuration = settings.SettleDuration * durationScale;

        mainStartRotation = currentMainRotation;
        mainBendRotation = mainBaseRotation * Quaternion.AngleAxis(bendAngle, bendAxis);
        mainOvershootRotation = mainBaseRotation * Quaternion.AngleAxis(-overshootAngle, bendAxis);

        sequence = DOTween.Sequence();
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainBendRotation, mainBendDuration).SetEase(Ease.OutQuad));
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainOvershootRotation, mainOvershootDuration).SetEase(Ease.InOutQuad));
        sequence.Append(view.ReactionRoot.DOLocalRotateQuaternion(mainBaseRotation, mainSettleDuration).SetEase(Ease.OutCubic));

        if (hasSecondaryLag)
        {
            var crownAngle = bendAngle * settings.CrownAngleMultiplier;
            var crownOvershoot = overshootAngle * settings.CrownAngleMultiplier;
            var crownBendRotation = crownBaseRotation * Quaternion.AngleAxis(crownAngle, bendAxis);
            var crownOvershootRotation = crownBaseRotation * Quaternion.AngleAxis(-crownOvershoot, bendAxis);

            sequence.Insert(settings.CrownLag,
                view.CrownRoot.DOLocalRotateQuaternion(crownBendRotation, mainBendDuration).From(currentCrownRotation).SetEase(Ease.OutQuad));
            sequence.Insert(settings.CrownLag + mainBendDuration,
                view.CrownRoot.DOLocalRotateQuaternion(crownOvershootRotation, mainOvershootDuration).SetEase(Ease.InOutQuad));
            sequence.Insert(settings.CrownLag + mainBendDuration + mainOvershootDuration,
                view.CrownRoot.DOLocalRotateQuaternion(crownBaseRotation, mainSettleDuration).SetEase(Ease.OutCubic));
        }

        sequence.OnComplete(ResetToNeutral);
        sequence.SetLink(view.gameObject);
    }

    public void ResetToNeutral()
    {
        KillActiveSequence();
        view.ReactionRoot.localRotation = mainBaseRotation;

        if (hasSecondaryLag)
        {
            view.CrownRoot.localRotation = crownBaseRotation;
        }
    }

    private void CacheBasePose()
    {
        mainBaseRotation = view.ReactionRoot.localRotation;
        crownBaseRotation = view.CrownRoot.localRotation;
    }

    private Vector3 GetAwayDirectionLocal(Vector3 sourceWorldPosition)
    {
        var pivotPosition = view.ReactionRoot.position;
        var awayWorld = pivotPosition - sourceWorldPosition;
        awayWorld.y = 0f;

        if (awayWorld.sqrMagnitude < 0.0001f)
        {
            awayWorld = view.transform.forward;
            awayWorld.y = 0f;
        }

        awayWorld.Normalize();

        var parent = view.ReactionRoot.parent;
        var awayLocal = parent.InverseTransformDirection(awayWorld);

        awayLocal.y = 0f;
        if (awayLocal.sqrMagnitude < 0.0001f)
        {
            awayLocal = Vector3.forward;
        }

        return awayLocal.normalized;
    }

    private void KillActiveSequence()
    {
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill(false);
        }
    }

    private void PlayLeavesBursts(bool isFinalHit)
    {
        var leavesPoints = view.LeavesPoints;
        if (leavesPoints == null || leavesPoints.Length == 0 || ParticlePool.Instance == null)
        {
            return;
        }

        var hitChance = isFinalHit
            ? Mathf.Min(1f, settings.LeavesHitChance + 0.2f)
            : settings.LeavesHitChance;

        if (Random.value > hitChance)
        {
            return;
        }

        var maxBursts = Mathf.Max(settings.MinLeavesBursts, settings.MaxLeavesBursts);
        var burstsCount = Random.Range(settings.MinLeavesBursts, maxBursts + 1);
        if (isFinalHit && burstsCount < maxBursts)
        {
            burstsCount++;
        }

        for (int i = 0; i < burstsCount; i++)
        {
            var point = leavesPoints[Random.Range(0, leavesPoints.Length)];
            var offset = Random.insideUnitSphere * settings.LeavesPositionJitter;
            ParticlePool.Instance.PlayFallenLeaves(point.position + offset);
        }
    }

}
