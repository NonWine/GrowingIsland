using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private EnemyFactory enemyFactory;
    [SerializeField] private EnemyConfig  config;
    [SerializeField] private Transform startPoint;
    void Start()
    {
      var enemy =  enemyFactory.Create(config);
      enemy.transform.position = startPoint.position;
      enemy.name = config.name + config.guId;
      enemy.transform.SetParent(startPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
