using UnityEngine;

[CreateAssetMenu(menuName = "Create EnemyConfig", fileName = "EnemyConfig", order = 0)]
public class EnemyConfig : ScriptableObject
{
    public BaseEnemy EnemyPrefab;
    public int MaxCount;

}