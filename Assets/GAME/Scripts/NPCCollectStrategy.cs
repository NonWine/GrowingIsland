using System;
using DG.Tweening;
using UnityEngine;

public class NPCCollectStrategy : ICollectDestroyStrategy
{
    public void Collect(ResourcePartObj resource, Transform collector, Action onImpact = null)
    {
        if (resource.UseStylizedMagnetPickup)
        {
            ResourceMagnetPickupAnimator.Play(resource, collector, punchCollector: true, onImpact);
            return;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(resource.transform.DOMove(collector.position + Vector3.up, 0.3f).SetEase(Ease.InQuad));
        seq.Join(resource.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            onImpact?.Invoke();
            resource.gameObject.SetActive(false);
        });
    }
}
