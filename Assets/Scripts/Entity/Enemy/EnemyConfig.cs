using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyConfig", fileName = "EnemyConfig", order = 0)]
public class EnemyConfig : ScriptableObject
{

    
    [Space(20)]
    
    
    [HideLabel] [HorizontalGroup(width:0.3f)] [ValueDropdown("GetAllScriptableObjects", AppendNextDrawer = true)] [PreviewField(180, ObjectFieldAlignment.Left)] 
    public GameObject EnemyPrefab;
    
    [Space(20)]

    
    [HorizontalGroup(width:0.7f)] [InlineProperty] [HideLabel] [SerializeField] 
    public EnemyStats enemyStats;

    

    private IEnumerable<GameObject> GetAllScriptableObjects() => EnemyConfigResolver.GetAllScriptableObjects();
}


