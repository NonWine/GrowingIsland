using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private GameObjectContext context;
    
    [Inject] private EnemyConfig config;
  
        
    private void Start()
    {
        var enemy = context.Container.InstantiatePrefab(config.EnemyPrefab, transform);
     
        if (enemy == null)
        {
            return;
        }

        enemy.transform.position = transform.position;
        enemy.name = config.name + config.guId;
        enemy.transform.SetParent(transform);
    }
}
