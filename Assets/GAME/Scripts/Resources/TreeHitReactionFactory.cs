using Zenject;

public class TreeHitReactionFactory : ITreeHitReactionFactory
{
    private readonly DiContainer container;

    public TreeHitReactionFactory(DiContainer container)
    {
        this.container = container;
    }

    public ITreeHitReaction Create(EnvironmentPropObjectView view, TreeHitAnimationSettings settings)
    {
        return container.Instantiate<TreeHitReaction>(new object[] { view, settings });
    }
}
