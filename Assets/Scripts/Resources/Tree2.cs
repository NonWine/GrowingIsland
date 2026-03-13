using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Tree2 : EnvironmentResource
{
    [FormerlySerializedAs("_animSettings")]
    [SerializeField] private TreeHitAnimationSettings animSettings = new();

    private Vector3 baseScale;
    private Quaternion baseRotation;
    private bool hasDefaults;

    protected override void AnimTrigDamage()
    {
        if (!hasDefaults)
        {
            baseScale = transform.localScale;
            baseRotation = transform.localRotation;
            hasDefaults = true;
        }

        transform.DOKill();
        transform.localRotation = baseRotation;
        transform.localScale = baseScale;

        var swayDir2D = Random.insideUnitCircle;
        if (swayDir2D == Vector2.zero)
        {
            swayDir2D = Vector2.right;
        }

        var swayDir = new Vector3(swayDir2D.x, 0f, swayDir2D.y).normalized;
        var leanedRotation = baseRotation * Quaternion.Euler(swayDir * animSettings.LeanAngle);

        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotateQuaternion(leanedRotation, animSettings.HitPushDuration).SetEase(Ease.OutQuad));
        seq.Join(transform.DOScale(baseScale * animSettings.ScaleBump, animSettings.HitPushDuration).SetEase(Ease.OutSine));
        seq.Append(transform.DOLocalRotateQuaternion(baseRotation, animSettings.ReturnDuration).SetEase(Ease.OutElastic, animSettings.ReboundElasticity, animSettings.ReboundPeriod));
        seq.Join(transform.DOScale(baseScale, animSettings.ReturnDuration).SetEase(Ease.OutElastic, animSettings.ReboundElasticity, animSettings.ReboundPeriod));
        seq.Append(transform.DOShakeRotation(animSettings.ShakeDuration, animSettings.ShakeStrength, animSettings.ShakeVibrato, animSettings.ShakeRandomness, true));
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        ParticlePool.Instance.PlayAxeHitFx(transform.position);
    }
}
