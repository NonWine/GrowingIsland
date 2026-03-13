using System;

public sealed class SawmillPileVisualizer : ISawmillPileVisualizer, IDisposable
{
    private readonly ISawmillPileVisualTarget _view;
    private readonly SawmillPileRuntime _runtime;
    private readonly ISawmillPileLayoutCalculator _layoutCalculator;
    private readonly ISawmillPileStageFactory _stageFactory;
    private readonly ISawmillPileAnimator _animator;

    public SawmillPileVisualizer(
        ISawmillPileVisualTarget view,
        SawmillPileRuntime runtime,
        ISawmillPileLayoutCalculator layoutCalculator,
        ISawmillPileStageFactory stageFactory,
        ISawmillPileAnimator animator)
    {
        _view = view;
        _runtime = runtime;
        _layoutCalculator = layoutCalculator;
        _stageFactory = stageFactory;
        _animator = animator;
    }

    public void Dispose()
    {
        _runtime.Clear();
    }

    public void RenderStorage(int current, int capacity, bool animateStageChange)
    {
        EnsureStagesBuilt(capacity);

        int visibleStages = _layoutCalculator.GetVisibleStageCount(
            _view.PileVisualSettings,
            current,
            capacity,
            _runtime.StageRoots.Count);

        _animator.ApplyVisibility(
            _runtime.StageRoots,
            _view.PileVisualSettings,
            visibleStages,
            animateStageChange);
    }

    public void PlayImpact(float impactStrength)
    {
        _animator.PlayImpact(_runtime.StageRoots, _view.ImpactFeedbackSettings, impactStrength);
    }

    private void EnsureStagesBuilt(int capacity)
    {
        SawmillPileVisualSettings settings = _view.PileVisualSettings;
        if (!settings.Enabled)
        {
            _runtime.Clear();
            return;
        }

        int stageCount = _layoutCalculator.GetStageCount(settings, capacity);
        if (_runtime.Matches(capacity, stageCount))
            return;

        _runtime.ReplaceStages(_stageFactory.CreateStages(_view, stageCount), capacity);
    }
}
