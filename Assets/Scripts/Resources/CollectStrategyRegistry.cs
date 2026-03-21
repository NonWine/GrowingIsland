using System.Collections.Generic;

public class CollectStrategyRegistry
{
    private readonly Dictionary<CollectStrategyType, ICollectDestroyStrategy> strategies;

    public CollectStrategyRegistry()
    {
        strategies = new Dictionary<CollectStrategyType, ICollectDestroyStrategy>
        {
            { CollectStrategyType.Player, new PlayerCollectStrategy() },
            { CollectStrategyType.NPC, new NPCCollectStrategy() },
            { CollectStrategyType.Auto, new DefaultCollectStrategy()}
        };
    }

    public ICollectDestroyStrategy GetStrategy(CollectStrategyType type)
    {
        if (strategies.TryGetValue(type, out var strategy))
        {
            return strategy;
        }
        return null;
    }
}
