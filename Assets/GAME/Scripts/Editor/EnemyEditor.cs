using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

    public class EnemyEditor : OdinMenuEditorWindow
    {
       [SerializeField] private EnemyEditorGeneralSettings editorGeneralSettings;
       private OdinMenuTree  odinMenuTree;
        [MenuItem("Custom/Enemy Editor")]
        private static void Open()
        {
            var window = GetWindow<EnemyEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            odinMenuTree.Selection.SelectionChanged -= SelectionOnSelectionChanged;

        }

        protected override OdinMenuTree BuildMenuTree()
        {
            odinMenuTree = new OdinMenuTree(true);
            odinMenuTree.DefaultMenuStyle.IconSize = 28.00f;
            odinMenuTree.Config.DrawSearchToolbar = true;
           var enemyTable = new EnemyTable(EnemyConfigAssetUtility.LoadAllEnemyConfigs());
            Debug.Log(enemyTable.AllEnemies.Count);
            // Adds the character overview table.
            odinMenuTree.Add("General", editorGeneralSettings);
            odinMenuTree.Add("Enemies", enemyTable);
         
            
            foreach (var enemy in enemyTable.AllEnemies)
            {
                Debug.Log(enemy.EnemyConfig.name); 
                odinMenuTree.Add($"Enemies/ {enemy.EnemyConfig.name}", enemy.EnemyConfig);

            }

            odinMenuTree.Selection.SelectionChanged += SelectionOnSelectionChanged;
            return odinMenuTree;
        }

        private void SelectionOnSelectionChanged(SelectionChangedType obj)
        {
            // Відпрацьовує при кліку на айтем у дереві
            var selected = odinMenuTree.Selection.FirstOrDefault();
            if (selected?.Value is EnemyConfig enemy)
            {
                Selection.activeObject = enemy;
                Debug.Log($"Selected enemy: {enemy.name}");
            }
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {

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

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete Enemy")))
                {
                    if (selected?.Value is EnemyConfig enemy)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(enemy);

                        
                        AssetDatabase.DeleteAsset(assetPath);
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }



