#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(Main))]
    public class MainEditor : OdinEditor
    {
        private Main main;

        private void OnEnable()
        {
            main = (Main)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginDisabledGroup(main.configInit != null && main.mainModules != null);
            if (GUILayout.Button("GetModuleAndConfig"))
            {
                FindConfigAndModuleFromAsset();
            } 
            EditorGUI.EndDisabledGroup();
            
            GUILayout.Space(3);
            // disable the button if the config and module are already set
            EditorGUI.BeginDisabledGroup(main.configInit != null && main.mainModules != null);
            if(GUILayout.Button("CreateModuleAndConfig"))
            {
                CreateConfigAndModule();
            }
            EditorGUI.EndDisabledGroup();
        }
        
        private void CreateConfigAndModule()
        {
            ConfigInit configInit   = CreateInstance<ConfigInit>();
            MainModules mainModules = CreateInstance<MainModules>();
            
            string path = "Assets/OSK/Resources/Configs";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets/OSK/Resources", "Configs");
            }
            
            IOUtility.CreateDirectory(path);
            if (main.configInit == null)
            {
                AssetDatabase.CreateAsset(configInit, $"{path}/ConfigInit.asset");
                main.configInit = configInit;
            }

            if (main.mainModules == null)
            {
                AssetDatabase.CreateAsset(mainModules, $"{path}/MainModules.asset");
                main.mainModules = mainModules;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
         
        private void FindConfigAndModuleFromAsset()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ConfigInit");
            string[] guidModules = UnityEditor.AssetDatabase.FindAssets("t:MainModules");
 
            ConfigInit configInit = null;
            MainModules mainModules = null;
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                configInit = UnityEditor.AssetDatabase.LoadAssetAtPath<ConfigInit>(path);
            }

            foreach (var guid in guidModules)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                mainModules = UnityEditor.AssetDatabase.LoadAssetAtPath<MainModules>(path);
            }
            
            if (configInit == null)
            {
                Debug.LogError("No ConfigInit found in the project.");
            }
            
            if (mainModules == null)
            {
                Debug.LogError("No MainModules found in the project.");
            }
            
            main.configInit = configInit;
            main.mainModules = mainModules;
            
            // Nếu object là prefab, ghi nhận thay đổi để tránh mất ref
            if (PrefabUtility.IsPartOfPrefabInstance(main))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(main);
            }
            EditorUtility.SetDirty(main);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif