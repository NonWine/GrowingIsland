using DG.Tweening;
using UnityEngine;

public class Tree2 : EnvironmentResource
{
    [SerializeField] private TreeHitAnimationSettings _animSettings = new();

    private Vector3 _baseScale;
    private Quaternion _baseRotation;
    private bool _hasDefaults;

    protected override void AnimTrigDamage()
    {
        if (!_hasDefaults)
        {
            _baseScale = transform.localScale;
            _baseRotation = transform.localRotation;
            _hasDefaults = true;
        }

        transform.DOKill();
        transform.localRotation = _baseRotation;
        transform.localScale = _baseScale;

        var swayDir2D = Random.insideUnitCircle;
        if (swayDir2D == Vector2.zero)
        {
            swayDir2D = Vector2.right;
        }

        var swayDir = new Vector3(swayDir2D.x, 0f, swayDir2D.y).normalized;
        var leanedRotation = _baseRotation * Quaternion.Euler(swayDir * _animSettings.LeanAngle);

        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotateQuaternion(leanedRotation, _animSettings.HitPushDuration).SetEase(Ease.OutQuad));
        seq.Join(transform.DOScale(_baseScale * _animSettings.ScaleBump, _animSettings.HitPushDuration).SetEase(Ease.OutSine));
        seq.Append(transform.DOLocalRotateQuaternion(_baseRotation, _animSettings.ReturnDuration).SetEase(Ease.OutElastic, _animSettings.ReboundElasticity, _animSettings.ReboundPeriod));
        seq.Join(transform.DOScale(_baseScale, _animSettings.ReturnDuration).SetEase(Ease.OutElastic, _animSettings.ReboundElasticity, _animSettings.ReboundPeriod));
        seq.Append(transform.DOShakeRotation(_animSettings.ShakeDuration, _animSettings.ShakeStrength, _animSettings.ShakeVibrato, _animSettings.ShakeRandomness, true));
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        ParticlePool.Instance.PlayAxeHitFx(transform.position);
    }
}
