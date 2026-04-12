using System;
using DG.Tweening;
using Zenject;


public sealed class ScalePunchEnvironmentResourceDamageFeedback
{
    private readonly EnvironmentPropObjectView view;

    public ScalePunchEnvironmentResourceDamageFeedback(EnvironmentPropObjectView view)
    {
        this.view = view;
    }
    

    private void OnHitApplied(EnvironmentResourceHitResult result)
    {
        view.transform.DOKill();
        view.transform.DOScale(1.05f, 0.15f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                view.transform.DOScale(1f, 0.15f)
                    .SetEase(Ease.OutBounce);
            });
    }
}
