using System;
using DG.Tweening;
using UnityEngine;

public class DefaultCollectStrategy : ICollectDestroyStrategy
{
    public void Collect(ResourcePartObj resource, Transform collector, Action onImpact = null)
    {
        resource.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onImpact?.Invoke();
            resource.gameObject.SetActive(false);
        });
    }
}
