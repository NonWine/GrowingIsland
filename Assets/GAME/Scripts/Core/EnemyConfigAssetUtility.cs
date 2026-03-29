using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class EnemyConfigAssetUtility
{
    public static IEnumerable<EnemyConfig> LoadAllEnemyConfigs()
    {
        return AssetDatabase.FindAssets("t:EnemyConfig")
            .Select(guid => AssetDatabase.LoadAssetAtPath<EnemyConfig>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(config => config != null)
            .ToArray();
    }
}
