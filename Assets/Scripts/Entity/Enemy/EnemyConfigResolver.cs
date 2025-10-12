using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static  class EnemyConfigResolver
{
    public static IEnumerable<GameObject> GetAllScriptableObjects()
    {// Шукаємо тільки префаби з компонентом BaseEnemy
        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/prefab/Enemy" });
    
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
            if (prefab != null && prefab.GetComponent<BaseEnemy>() != null)
            {
                yield return prefab;
            }
        }
    }
}