using DG.Tweening;
using UnityEngine;

public class DefaultCollectStrategy : ICollectDestroyStrategy
{
    public void Collect(Transform resource, Transform collector)
    {
        resource.DOScale(0f, 0.25f).SetEase(Ease.InBack).OnComplete(() => { resource.gameObject.SetActive(false); });
    }
}