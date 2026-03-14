using Zenject;

public class TreeHitReactionFactory : ITreeHitReactionFactory
{
    private readonly DiContainer container;

    public TreeHitReactionFactory(DiContainer container)
    {
        this.container = container;
    }

    public ITreeHitReaction Create(TreeView view, TreeHitAnimationSettings settings)
    {
        return container.Instantiate<TreeHitReaction>(new object[] { view, settings });
    }
}
