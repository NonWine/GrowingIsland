using System.Collections.Generic;
using Zenject;
public class EnemyFactory : IFactory<EnemyConfig, BaseEnemy>
{
    private readonly DiContainer _container;
    private readonly List<EnemyConfig> _configs;

    public EnemyFactory(DiContainer container, List<EnemyConfig> configs)
    {
        _container = container;
        _configs = configs;
    }

    public BaseEnemy Create(EnemyConfig config)
    {
        var tryGetConfig = _configs.Find(x => x.guId == config.guId);

        if (tryGetConfig != null)
        {
            var enemyStats = tryGetConfig.enemyStats;
            
            var instance = _container.InstantiatePrefabForComponent<BaseEnemy>(tryGetConfig.EnemyPrefab, 
                new object[] {enemyStats});
            return instance;
        }

        return null;
    }
}
