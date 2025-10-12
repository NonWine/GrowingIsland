using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

    public class EnemyEditor : OdinMenuEditorWindow
    {
       [SerializeField] private EnemyEditorGeneralSettings editorGeneralSettings;
       private EnemyTable enemyTable;
       
        [MenuItem("Custom/Enemy Editor")]
        private static void Open()
        {
            var window = GetWindow<EnemyEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;
            enemyTable = new EnemyTable(UpdateEnemiesFromFolder());
            Debug.Log(enemyTable.AllEnemies.Count);
            // Adds the character overview table.
            tree.Add("General", editorGeneralSettings);
            tree.Add("Enemies", enemyTable);
         
            foreach (var enemy in enemyTable.AllEnemies)
            {
                Debug.Log(enemy.EnemyConfig.name); 
                tree.Add($"Enemies/ {enemy.EnemyConfig.name}", enemy.EnemyConfig);

            }
            
            return tree;
        }
        
        
        public IEnumerable<EnemyConfig> UpdateEnemiesFromFolder()
        {
            // Finds and assigns all scriptable objects of type Character
            var enemies = AssetDatabase.FindAssets("t:EnemyConfig")
                .Select(guid => AssetDatabase.LoadAssetAtPath<EnemyConfig>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            
            return enemies;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;


            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    Debug.Log(selected.Name);
                    GUILayout.Label(selected.Name);
                    if (selected.Value is EnemyConfig enemy)
                    {
                            
                        EditorUtility.FocusProjectWindow();
                        Selection.activeObject = enemy;
                    }
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Enemy")))
                {
                    string folderPath = AssetDatabase.GetAssetPath(editorGeneralSettings.enemyConfigFolder);
                    string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewEnemyConfig.asset");
                    var asset = ScriptableObject.CreateInstance<EnemyConfig>();
                    
                    AssetDatabase.CreateAsset(asset, assetPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
        
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;
                }

            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }



