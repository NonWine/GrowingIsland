using System;
using DG.Tweening;

public sealed class TreeTweenDelayScheduler : ITreeDelayScheduler
{
    private readonly EnvironmentPropObjectView view;

    public TreeTweenDelayScheduler(EnvironmentPropObjectView view)
    {
        this.view = view;
    }

    public ITreeScheduledAction Schedule(float delaySeconds, Action callback)
    {
        Tween tween = DOVirtual.DelayedCall(delaySeconds, () => callback?.Invoke())
            .SetLink(view.gameObject);
        return new TreeTweenScheduledAction(tween);
    }

    private sealed class TreeTweenScheduledAction : ITreeScheduledAction
    {
        private Tween tween;

        public TreeTweenScheduledAction(Tween tween)
        {
            this.tween = tween;
        }

        public void Cancel()
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill(false);
            }

            tween = null;
        }
    }
}