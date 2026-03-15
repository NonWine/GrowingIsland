using System;
using DG.Tweening;
using UnityEngine;

public class PlayerCollectStrategy : ICollectDestroyStrategy
{
    public void Collect(ResourcePartObj resource, Transform collector, Action onImpact = null)
    {
        if (resource.UseStylizedMagnetPickup)
        {
            ResourceMagnetPickupAnimator.Play(resource, collector, punchCollector: false, onImpact);
            return;
        }

        resource.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onImpact?.Invoke();
            resource.gameObject.SetActive(false);
        });
    }
}
