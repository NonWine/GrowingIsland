using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyTable
{
    [TableList] [ShowInInspector]
    public  List<EnemyWrapper> AllEnemies {get; private set; }

    
    public EnemyConfig this[int index]
    {
        get { return this.AllEnemies[index].EnemyConfig; }
    }

    public EnemyTable(IEnumerable<EnemyConfig> enemyConfigs)
    {
        this.AllEnemies = enemyConfigs.Select(x => new EnemyWrapper(x)).ToList();
    }

    public class EnemyWrapper
    {
       private EnemyConfig enemyConfig;

        [ShowInInspector]
        public EnemyConfig EnemyConfig
        {
            get { return this.enemyConfig; }
        }

        public EnemyWrapper(EnemyConfig enemyConfig)
        {
            this.enemyConfig = enemyConfig;
        }

        [TableColumnWidth(50, false)]
        [ShowInInspector, PreviewField(45, ObjectFieldAlignment.Center)]
        public GameObject model { get { return this.enemyConfig.EnemyPrefab; } set { this.enemyConfig.EnemyPrefab = value; EditorUtility.SetDirty(this.enemyConfig); } }


        [ShowInInspector]
        public EnemyStats EnemyStats { get { return this.enemyConfig.enemyStats; } set { this.enemyConfig.enemyStats = value; EditorUtility.SetDirty(this.enemyConfig); } }
        
    }
}