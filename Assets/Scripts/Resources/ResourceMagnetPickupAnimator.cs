using System;
using DG.Tweening;
using UnityEngine;

public static class ResourceMagnetPickupAnimator
{
    public static void Play(ResourcePartObj resource, Transform collector, bool punchCollector, Action onImpact)
    {
        if (resource == null)
            return;

        Transform resourceTransform = resource.transform;
        resourceTransform.DOKill(complete: false);

        if (collector == null)
        {
            Sequence fallbackSequence = DOTween.Sequence();
            fallbackSequence.SetLink(resource.gameObject);
            fallbackSequence.Append(resourceTransform.DOScale(0f, 0.12f).SetEase(Ease.InBack));
            fallbackSequence.OnComplete(() =>
            {
                onImpact?.Invoke();
                resource.gameObject.SetActive(false);
            });
            return;
        }

        Vector3 startPosition = resourceTransform.position;
        Vector3 endPosition = collector.position;
        Vector3 direction = endPosition - startPosition;
        Vector3 sideAxis = Vector3.Cross(Vector3.up, direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.forward);
        if (sideAxis.sqrMagnitude <= 0.0001f)
            sideAxis = Vector3.right;

        float sideOffset = Mathf.Clamp(direction.magnitude * 0.08f, 0.025f, 0.08f);
        float sideSign = ((resource.GetInstanceID() & 1) == 0) ? -1f : 1f;
        Vector3 apexPosition = Vector3.Lerp(startPosition, endPosition, 0.5f)
                               + Vector3.up * resource.PickupArcHeight
                               + sideAxis.normalized * sideOffset * sideSign;

        float riseDuration = resource.PickupFlyDuration * 0.58f;
        float descendDuration = Mathf.Max(0.04f, resource.PickupFlyDuration - riseDuration);
        float popDuration = Mathf.Max(0.01f, resource.FinalPickupPopDuration);
        Vector3 baseScale = resourceTransform.localScale;
        Vector3 midScale = baseScale * 1.06f;
        Vector3 impactScale = baseScale * resource.FinalPickupPopScale;

        Sequence sequence = DOTween.Sequence();
        sequence.SetLink(resource.gameObject);
        if (resource.PickupDelay > 0f)
            sequence.AppendInterval(resource.PickupDelay);

        sequence.Append(resourceTransform.DOMove(apexPosition, riseDuration).SetEase(Ease.OutCubic));
        sequence.Append(resourceTransform.DOMove(endPosition, descendDuration).SetEase(Ease.InQuad));

        Sequence travelScaleSequence = DOTween.Sequence();
        if (resource.PickupDelay > 0f)
            travelScaleSequence.AppendInterval(resource.PickupDelay);
        travelScaleSequence.Append(resourceTransform.DOScale(midScale, riseDuration).SetEase(Ease.OutSine));
        travelScaleSequence.Append(resourceTransform.DOScale(baseScale, descendDuration).SetEase(Ease.InSine));
        sequence.Join(travelScaleSequence);

        sequence.AppendCallback(() =>
        {
            resourceTransform.position = collector.position;
            resourceTransform.localScale = impactScale;
            onImpact?.Invoke();

            if (ParticlePool.Instance != null)
                ParticlePool.Instance.PlayPoof(resourceTransform.position);

            if (punchCollector)
            {
                collector.DOPunchScale(
                        Vector3.one * Mathf.Max(0f, resource.FinalPickupPopScale - 1f),
                        popDuration,
                        vibrato: 1,
                        elasticity: 0f)
                    .SetLink(collector.gameObject);
            }
        });

        sequence.Append(resourceTransform.DOScale(0f, popDuration).SetEase(Ease.InBack));
        sequence.OnComplete(() => resource.gameObject.SetActive(false));
    }
}
