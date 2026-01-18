using DG.Tweening;
using UnityEngine;

public class NPCCollectStrategy : ICollectDestroyStrategy
{
    public void Collect(Transform resource, Transform collector)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(resource.DOMove(collector.position + Vector3.up, 0.3f).SetEase(Ease.InQuad));
        seq.Join(resource.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() => { resource.gameObject.SetActive(false); });
    }
}