public sealed class PlayerFarmTargetTracker
{
    public EnvironmentPropObjectView CurrentTarget { get; private set; }

    public bool HasTarget => CurrentTarget != null && CurrentTarget.IsAlive;

    public void SetTarget(EnvironmentPropObjectView target)
    {
        CurrentTarget = target;
    }

    public void Clear()
    {
        CurrentTarget = null;
    }
}
