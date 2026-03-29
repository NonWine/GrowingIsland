using System.Collections.Generic;
using Zenject;
public class EnemyFactory : IFactory<EnemyConfig, BaseEnemy>
{
    private readonly DiContainer container;
    private readonly List<EnemyConfig> configs;

    public EnemyFactory(DiContainer container, List<EnemyConfig> configs)
    {
        this.container = container;
        this.configs = configs;
    }

    public BaseEnemy  Create(DiContainer container, EnemyConfig config)
{
    return Create(container, config);
}

public BaseEnemy Create(DiContainer container, EnemyConfig config, params object[] extraArgs)
{
    var tryGetConfig = configs.Find(x => x.guId == config.guId);

    if (tryGetConfig != null)
    {
        var enemyStats = tryGetConfig.enemyStats;
        container.Bind<EnemyStats>().FromInstance(enemyStats).AsCached();
        var args = new List<object> { enemyStats };
        if (extraArgs != null && extraArgs.Length > 0)
        {
            args.AddRange(extraArgs);
        }

        var instance = container.InstantiatePrefabForComponent<BaseEnemy>(
            tryGetConfig.EnemyPrefab);

        return instance;
    }

    return null;
}

    public BaseEnemy Create(EnemyConfig param)
    {
        return Create(container, param);
    }

}

