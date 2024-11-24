using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class MobSpawnerService : MonoBehaviour
{
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private Transform _spawnPoint;
    //[Inject] private ObjectPoolEnemy _objectPoolEnemy;

    private void Start()
    {
        for (int i = 0; i < _enemyConfig.MaxCount; i++)
        {
            Spawn();

        }
    }
   // [Button]
    private void Spawn()
    {
        Transform point = _spawnPoint;
        float randomAngle = Random.Range(0f, 2 * Mathf.PI);
        point.position = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * Random.Range(5,10); 
  //      var enemy = _objectPoolEnemy.SpawnEnemy(_enemyConfig.EnemyPrefab, point, Quaternion.identity);
     //   enemy.OnDie += Respawn;
    }

    private async void Respawn(BaseEnemy enemy)
    {
        enemy.OnDie -= Respawn;
        await UniTask.Delay(Random.Range(2000, 5000));
        Spawn();
    }
}