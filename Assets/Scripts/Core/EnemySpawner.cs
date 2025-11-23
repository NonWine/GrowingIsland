using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private EnemyFactory enemyFactory;
    
    [SerializeField, HideLabel]
    [ValueDropdown(nameof(GetEnemyConfigs), IsUniqueList = true, DrawDropdownForListElements = false,
        ExcludeExistingValuesInList = true)]
    private EnemyConfig config;
  
    
    [SerializeField] private Transform startPoint;
    
    private void Start()
    {
      var enemy =  enemyFactory.Create(config);
      enemy.transform.position = transform.position;
      enemy.name = config.name + config.guId;
      enemy.transform.SetParent(startPoint);
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
