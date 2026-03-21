public sealed class TreeFinalFallTiming
{
    public float ImpactDelay { get; }
    public float CompletionDelay { get; }

    public TreeFinalFallTiming(TreeFinalFallSettings settings)
    {
        ImpactDelay = settings.MicroHoldDuration + settings.MicroHoldDuration + settings.FallDuration;
        CompletionDelay = settings.LandImpactDuration;
    }
}