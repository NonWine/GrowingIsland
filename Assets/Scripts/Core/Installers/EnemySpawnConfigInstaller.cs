using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class EnemySpawnConfigInstaller : MonoInstaller
{

      [SerializeField, HideLabel]
    [ValueDropdown(nameof(GetEnemyConfigs), IsUniqueList = true, DrawDropdownForListElements = false,
        ExcludeExistingValuesInList = true)]
    private EnemyConfig config;
  
    public override void InstallBindings()
    {
            Container.Bind<EnemyStats>().FromInstance(config.enemyStats).AsSingle();
            Container.Bind<EnemyConfig>().FromInstance(config).AsSingle();
    }

    
    #if UNITY_EDITOR
        private static IEnumerable<EnemyConfig> GetEnemyConfigs()
        {
            return EnemyConfigAssetUtility.LoadAllEnemyConfigs();
        }
    #else
        private static IEnumerable<EnemyConfig> GetEnemyConfigs()
        {
            return System.Array.Empty<EnemyConfig>();
        }
    #endif
}