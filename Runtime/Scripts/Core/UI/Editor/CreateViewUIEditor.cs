#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class CreateViewUIEditor : EditorWindow
    {
        private string scriptName = "NewView";
        private static bool isAlerpView = false;
        private static bool isCreateShield = true;
        private static string scriptPath;

        [MenuItem("OSK-Framework/UI/Create view")]
        public static void ShowWindow()
        {
            GetWindow<CreateViewUIEditor>("Create View");
        }
        

        private void OnEnable()
        {
            scriptPath = EditorPrefs.GetString("ScriptPathView", "Assets/");
        }

        private void OnDisable()
        {
            EditorPrefs.SetString("ScriptPathView", scriptPath);
        }

        private void OnGUI()
        {
            GUILayout.Label("Create New View", EditorStyles.boldLabel);

            scriptName = EditorGUILayout.TextField("Script Name", scriptName);
            isAlerpView = EditorGUILayout.Toggle("Is Alert View", isAlerpView);
            isCreateShield = EditorGUILayout.Toggle("Create Shield", isCreateShield);


            if (GUILayout.Button("Create View"))
            {
                CreateAndAttachView(scriptName);
            }


            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Select Folder Path", EditorStyles.boldLabel);
            EditorGUILayout.TextField("Folder Path", scriptPath);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Folder", scriptPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    path = "Assets" + path.Replace(Application.dataPath, "");
                    scriptPath = path;
                    EditorPrefs.SetString("ScriptPathView", scriptPath); 
                    EditorUtility.SetDirty(this);
                }
            }

            if (GUILayout.Button("Open Folder", GUILayout.Width(100)))
            { 
                EditorUtility.RevealInFinder(scriptPath);
            }

            if (GUILayout.Button("Clear", GUILayout.Width(100)))
            {
                scriptPath = "Assets/";
                EditorPrefs.SetString("ScriptPath", scriptPath);
                EditorUtility.SetDirty(this);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void CreateAndAttachView(string scriptName)
        {
            if (string.IsNullOrEmpty(scriptName))
            {
                EditorUtility.DisplayDialog("Error", "Script name cannot be empty.", "OK");
                return;
            }

            if (!File.Exists(scriptPath))
            {
                CreateScriptFile(scriptPath, scriptName);
                AssetDatabase.ImportAsset(scriptPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            EditorApplication.update += WaitForCompilation;
        }

        private static void WaitForCompilation()
        {
            if (!EditorApplication.isCompiling)
            {
                EditorApplication.update -= WaitForCompilation;
                Debug.Log("Compilation completed, proceeding to attach script.");
                WaitForCompilationAndAttach();
            }
        }

        private static void WaitForCompilationAndAttach()
        {
            MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            if (monoScript == null)
            {
                Debug.LogError($"Failed to load MonoScript at '{scriptPath}'.");
                return;
            }

            System.Type scriptType = monoScript.GetClass();
            if (scriptType == null)
            {
                Debug.LogError($"Failed to get class type for script. Check for compilation errors.");
                return;
            }

            AttachScriptToGameObject(scriptType);
        }

        private static void AttachScriptToGameObject(System.Type scriptType)
        {
            GameObject view = CreateRootView(scriptType.Name, scriptType);
            if (view != null)
            {
                if (isCreateShield)
                {
                    CreateShield(view.transform);
                }

                CreateContainer(view);
                Selection.activeGameObject = view;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Created GameObject '{scriptType.Name}' with script '{scriptType.Name}' attached.");
        }

        private static GameObject CreateRootView(string scriptName, System.Type scriptType)
        {
            GameObject view = new GameObject(scriptName);
            view.layer = LayerMask.NameToLayer("UI");
            view.GetOrAdd<RectTransform>();
            view.GetOrAdd<CanvasRenderer>();
            view.AddComponent(scriptType);

            Transform parent = FindObjectOfType<RootUI>()?.GetViewContainer?.transform;
            view.transform.parent = parent;

            RectTransform rectTransform = view.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            return view;
        }

        private static void CreateContainer(GameObject view)
        {
            GameObject container = new GameObject("Container");
            container.layer = LayerMask.NameToLayer("UI");
            container.transform.SetParent(view.transform);
            container.transform.localPosition = Vector3.zero;
            container.transform.localScale = Vector3.one;

            RectTransform containerRect = container.GetOrAdd<RectTransform>();
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void CreateShield(Transform parent)
        {
            GameObject shield = new GameObject("Shield");
            shield.layer = LayerMask.NameToLayer("UI");
            Image image = shield.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.9f);

            Transform t = shield.transform;
            t.SetParent(parent);
            t.SetSiblingIndex(0);
            t.localScale = Vector3.one;
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);

            RectTransform rt = t.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMax = new Vector2(5, 5);
            rt.offsetMin = new Vector2(-5, -5);
        }

        private static void CreateScriptFile(string path, string scriptName)
        {
            string viewType = isAlerpView ? "AlertView" : "View";
            string scriptTemplate = $@"
using UnityEngine;
using OSK;

public class {scriptName} : {viewType}
{{
    public override void Initialize(RootUI rootUI)
    {{
        base.Initialize(rootUI);
    }} 

    public override void Open(object[] data = null)
    {{
        base.Open(data);
    }}

    public override void Hide()
    {{
        base.Hide();
    }}
}}";
            File.WriteAllText(path, scriptTemplate);
            Debug.Log($"Script '{scriptName}' created at '{path}'.");
        }
    }
}
#endif