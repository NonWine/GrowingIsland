using System;

public sealed class SawmillPileVisualizer : IDisposable
{
    private readonly SawmillView view;
    private readonly SawmillPileRuntime runtime;
    private readonly SawmillPileLayoutCalculator layoutCalculator;
    private readonly SawmillPileStageFactory stageFactory;
    private readonly SawmillPileAnimator animator;
    private readonly SawmillPileVisualSettings pileSettings;
    private readonly SawmillImpactFeedbackSettings impactSettings;

    public SawmillPileVisualizer(
        SawmillView view,
        SawmillPileRuntime runtime,
        SawmillPileLayoutCalculator layoutCalculator,
        SawmillPileStageFactory stageFactory,
        SawmillPileAnimator animator,
        SawmillPileVisualSettings pileSettings,
        SawmillImpactFeedbackSettings impactSettings)
    {
        this.view = view;
        this.runtime = runtime;
        this.layoutCalculator = layoutCalculator;
        this.stageFactory = stageFactory;
        this.animator = animator;
        this.pileSettings = pileSettings;
        this.impactSettings = impactSettings;
    }

    public void Dispose()
    {
        runtime.Clear();
    }

    public void RenderStorage(int current, int capacity, bool animateStageChange)
    {
        EnsureStagesBuilt(capacity);

        int visibleStages = layoutCalculator.GetVisibleStageCount(
            pileSettings,
            current,
            capacity,
            runtime.StageRoots.Count);

        animator.ApplyVisibility(
            runtime.StageRoots,
            pileSettings,
            visibleStages,
            animateStageChange);
    }

    public void PlayImpact(float impactStrength)
    {
        animator.PlayImpact(runtime.StageRoots, impactSettings, impactStrength);
    }

    private void EnsureStagesBuilt(int capacity)
    {
        if (!pileSettings.Enabled)
        {
            runtime.Clear();
            return;
        }

        int stageCount = layoutCalculator.GetStageCount(pileSettings, capacity);
        if (runtime.Matches(capacity, stageCount))
            return;

        runtime.ReplaceStages(stageFactory.CreateStages(view, stageCount), capacity);
    }
}
